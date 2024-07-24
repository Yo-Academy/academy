using Academy.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Diagnostics;

namespace Academy.Infrastructure.Middleware
{
    public class RequestTracingMiddleware : IMiddleware
    {
        private readonly LoggerSettings _loggerSettings;
        public RequestTracingMiddleware(LoggerSettings loggerSettings)
        {
            _loggerSettings = loggerSettings ?? throw new ArgumentNullException(nameof(loggerSettings));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var stopWatch = Stopwatch.StartNew();
            if (_loggerSettings.IsRequestTracingEnabled) LogRequest(context);

            try
            {
                await next(context);
            }
            finally
            {
                if (_loggerSettings.IsRequestTracingEnabled) LogResponse(context, stopWatch);
            }
        }
        private void LogRequest(HttpContext context)
        {
            try
            {
                var request = context.Request;
                var requestEvent = new RequestTracingEvent
                {
                    Timestamp = DateTime.UtcNow,
                    HttpMethod = request.Method,
                    RequestUri = request.PathBase + request.Path
                };
                Log.ForContext("LogType", "RequestTrace").Information("Request trace {@requestEvent}", requestEvent);
            }
            catch (Exception)
            {
            }
        }

        private void LogResponse(HttpContext httpContext,
            Stopwatch stopWatch)
        {
            try
            {
                var durationMillseconds = stopWatch.ElapsedMilliseconds;
                var responseEvent = new ResponseTracingEvent
                {
                    Timestamp = DateTime.UtcNow,
                    HttpMethod = httpContext.Request.Method,
                    RequestUri = httpContext.Request.PathBase + httpContext.Request.Path,
                    HttpStatusCode = httpContext.Response.StatusCode,
                    DurationMillseconds = durationMillseconds
                };
                Log.ForContext("LogType", "ResponseTrace").Information("Response trace {@responseEvent}", responseEvent);
            }
            catch (Exception)
            {
            }
        }
    }
}
