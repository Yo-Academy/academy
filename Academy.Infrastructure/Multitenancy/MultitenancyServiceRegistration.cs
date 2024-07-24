using Academy.Application.Multitenancy;
using Academy.Infrastructure.Persistence;
using Academy.Shared.Authorization;
using Academy.Shared.Multitenancy;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Academy.Infrastructure.Multitenancy
{
    internal static class MultitenancyServiceRegistration
    {
        internal static IServiceCollection AddMultitenancy(this IServiceCollection services)
        {
            return services
                .AddDbContext<TenantDbContext>((p, options) =>
                {
                    // TODO: We should probably add specific dbprovider/connectionstring setting for the tenantDb with a fallback to the main databasesettings
                    var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                    options.UseDatabase(databaseSettings.DBProvider, databaseSettings.ConnectionString);
                })
                .AddMultiTenant<TenantInfo>()
                    .WithClaimStrategy(Claims.Tenant)
                    .WithHeaderStrategy(MultitenancyConstants.TenantIdName)
                    .WithQueryStringStrategy(MultitenancyConstants.TenantIdName)
                    .WithEFCoreStore<TenantDbContext, TenantInfo>()
                    .Services
                .AddScoped<ITenantService, TenantService>();
        }

        internal static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app) =>
            app.UseMultiTenant();

        private static FinbuckleMultiTenantBuilder<TenantInfo> WithQueryStringStrategy(this FinbuckleMultiTenantBuilder<TenantInfo> builder, string queryStringKey) =>
            builder.WithDelegateStrategy(context =>
            {
                if (context is not HttpContext httpContext)
                {
                    return Task.FromResult((string?)null);
                }

                httpContext.Request.Query.TryGetValue(queryStringKey, out StringValues tenantIdParam);

                return Task.FromResult((string?)tenantIdParam.ToString());
            });
    }
}
