using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using System.Security.Claims;

namespace Academy.Infrastructure.Logging.Serilog
{
    public class CustomLogEventEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomLogEventEnricher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Id", DefaultIdType.NewGuid()));
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("UserId", DefaultIdType.TryParse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out DefaultIdType userId) ? userId : default));
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("TenantId", httpContext.User.FindFirstValue(Academy.Shared.Authorization.Claims.Tenant) ?? string.Empty));
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("ClientIpAddress", httpContext.Connection.RemoteIpAddress?.ToString()));
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("RequestMethod", httpContext.Request?.Method));
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("RequestPath", $"{httpContext.Request?.Path}{(httpContext.Request.QueryString.HasValue ? httpContext.Request?.QueryString.Value : "")}"));
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("BrowserInfo", httpContext.Request?.Headers["User-Agent"].ToString()));
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("SourceContext", $"Controller: {httpContext.Request.RouteValues["controller"]?.ToString()}, Action: {httpContext.Request.RouteValues["action"]?.ToString()}"));
            }
        }
    }
}
