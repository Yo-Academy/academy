using Academy.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Academy.Infrastructure.Middleware
{
    public class RequestLoggingMiddleware : IMiddleware
    {
        private readonly LoggerSettings _loggerSettings;

        public RequestLoggingMiddleware(LoggerSettings loggerSettings)
        {
            _loggerSettings = loggerSettings ?? throw new ArgumentNullException(nameof(loggerSettings));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate _next)
        {
            Exception? exception = null;
            AuditLogs auditLogs = await LogRequest(context, new());

            var stopWatch = Stopwatch.StartNew();
            try
            {
                await _next(context);
                stopWatch.Stop();
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                var response = context.Response;
                response.ContentType = "application/json";

                exception = ex;
                var errorId = DefaultIdType.NewGuid();
                auditLogs.ErrorId = errorId;
                auditLogs.StatusCode = response.StatusCode;
                auditLogs.ExecutionDuration = stopWatch.ElapsedMilliseconds;

                var errorResult = new ErrorResult
                {
                    Source = exception.TargetSite?.DeclaringType?.FullName,
                    Exception = _loggerSettings.IsDevelopmentEnv ? ex?.ToString() : ex?.Message.Trim(),
                    ErrorId = errorId.ToString(),
                    SupportMessage = string.Format("Provide the ErrorId {0} to the support team for further analysis.", errorId)
                };

                if (exception is not CustomException && exception.InnerException != null)
                {
                    while (exception.InnerException != null)
                    {
                        exception = exception.InnerException;
                    }
                }

                if (exception is FluentValidation.ValidationException fluentException)
                {
                    errorResult.Exception = "One or More Validations failed.";
                    foreach (var error in fluentException.Errors)
                    {
                        errorResult.Messages.Add(error.ErrorMessage);
                    }
                }

                switch (exception)
                {
                    case CustomException e:
                        errorResult.StatusCode = (int)e.StatusCode;
                        if (e.ErrorMessages is not null)
                        {
                            errorResult.Messages = e.ErrorMessages;
                        }

                        break;

                    case KeyNotFoundException:
                        errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case FluentValidation.ValidationException:
                        errorResult.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    default:
                        errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                if (!response.HasStarted)
                {
                    response.ContentType = "application/json";
                    response.StatusCode = errorResult.StatusCode;
                    if (ex?.Message != null && errorResult.Messages.Count == 0)
                    {
                        errorResult.Messages.Add(ex.Message);
                    }
                    await response.WriteAsync(JsonConvert.SerializeObject(Result.Fail(errorResult.Messages.Select(x => new Error() { Message = x }).ToArray())));
                }
                else
                {
                    Log.Warning("Can't write error response. Response has already started.");
                }
            }
            finally
            {
                auditLogs.ExecutionDuration = stopWatch.ElapsedMilliseconds;
                auditLogs.StatusCode = context.Response?.StatusCode ?? 0;
                if (exception == null)
                {
                    SerilogService.Information("Audit event", auditLogs);
                }
                else
                {
                    SerilogService.Error(exception, "Exception", auditLogs);
                }
            }
        }

        private async Task<AuditLogs> LogRequest(HttpContext httpContext, AuditLogs auditLogs)
        {
            var request = httpContext.Request;
            auditLogs.ExecutionTime = DateTime.UtcNow;
            string requestBody = string.Empty;
            if (httpContext.Request.Path.ToString().Contains("tokens"))
            {
                requestBody = "[Redacted] Contains Sensitive Information.";
            }
            else
            {
                if (!string.IsNullOrEmpty(httpContext.Request.ContentType)
                    && httpContext.Request.ContentType.StartsWith("application/json"))
                {
                    httpContext.Request.EnableBuffering();
                    using var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 4096, true);
                    requestBody = await reader.ReadToEndAsync();
                    httpContext.Request.Body.Position = 0;

                    auditLogs.RequestBody = requestBody;
                    auditLogs.RequestHeaders = JsonConvert.SerializeObject(request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()));
                }
            }
            return auditLogs;
        }
    }
}
