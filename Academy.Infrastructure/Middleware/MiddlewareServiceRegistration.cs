using Academy.Infrastructure.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Academy.Infrastructure.Middleware
{
    internal static class MiddlewareServiceRegistration
    {
        internal static IServiceCollection AddRequestLogging(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            LoggerSettings configuration = GetLoggerSettings(config) ?? throw new ArgumentNullException(nameof(configuration));
            configuration.IsDevelopmentEnv = env.IsDevelopment();
            services.AddSingleton(configuration)
                .AddScoped<RequestLoggingMiddleware>();
            return services;
        }

        internal static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
            return app;
        }

        internal static IServiceCollection AddRequestTracingMiddleware(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            LoggerSettings configuration = GetLoggerSettings(config) ?? throw new ArgumentNullException(nameof(configuration));
            configuration.IsDevelopmentEnv = env.IsDevelopment();

            return services.AddSingleton(configuration)
                            .AddScoped<RequestTracingMiddleware>();
        }

        internal static IApplicationBuilder UseRequestTracingMiddleware(this IApplicationBuilder app) =>
          app.UseMiddleware<RequestTracingMiddleware>();

        private static LoggerSettings GetLoggerSettings(IConfiguration config) =>
        config.GetSection(nameof(LoggerSettings)).Get<LoggerSettings>()!;
    }
}
