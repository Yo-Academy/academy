using Academy.Application.Academies.Contracts;
using Academy.Application.Identity.Users;
using Academy.Application.Multitenancy;
using Academy.Infrastructure.Identity;
using Academy.Infrastructure.Persistence;
using Academy.Infrastructure.Persistence.Initialization;
using Academy.Shared.Authorization;
using Academy.Shared.Multitenancy;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Strategies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
                    .WithRouteStrategy(MultitenancyConstants.TenantIdName)
                    .WithHeaderStrategy(MultitenancyConstants.TenantIdName)
                    .WithQueryStringStrategy(MultitenancyConstants.TenantIdName)
                    .WithEFCoreStore<TenantDbContext, TenantInfo>()
                    .Services
                .AddScoped<ITenantService, TenantService>()
                .AddScoped<ITenantResolverService, TenantResolverService>();
        }

        internal static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app) =>
            app
            //.Use(async (context, next) =>
            //{
            //    // Try to get the tenant identifier dynamically
            //    var tenantResolverService = context.RequestServices.GetRequiredService<ITenantResolverService>();
            //    string? tenantId = await tenantResolverService.ResolveTenantIdAsync(context);

            //    if (!string.IsNullOrEmpty(tenantId))
            //    {
            //        var multiTenantContext = context.GetMultiTenantContext<TenantInfo>();

            //        // If the tenant context exists, update the TenantInfo dynamically
            //        if (multiTenantContext != null)
            //        {
            //            var tenantInfo = await tenantResolverService.GetTenantInfoAsync(tenantId);
            //            if (tenantInfo != null)
            //            {
            //                multiTenantContext.TenantInfo = tenantInfo;
            //                Console.WriteLine($"Tenant switched to: {tenantInfo.Id}");
            //            }
            //            else
            //            {
            //                Console.WriteLine("Failed to find tenant info.");
            //            }
            //        }
            //        else
            //        {
            //            Console.WriteLine("No MultiTenantContext available.");
            //        }
            //    }

            //    await next(context);
            //})
            .UseMultiTenant();
        //.Use(async (context, next) =>
        //{
        //    // Save the original tenant before switching
        //    var originalTenantContext = context.GetMultiTenantContext<TenantInfo>();
        //    var originalTenantInfo = originalTenantContext?.TenantInfo;
        //    Console.WriteLine($"Original Tenant: {originalTenantInfo?.Id ?? "No tenant"}");
        //    var dbService = context.RequestServices.GetRequiredService<IDatabaseInitializer>();

        //    try
        //    {
        //        // Try to get the custom tenant identifier
        //        var tenantResolverService = context.RequestServices.GetRequiredService<ITenantResolverService>();

        //        string? customTenantId = await tenantResolverService.ResolveTenantIdAsync(context);

        //        if (!string.IsNullOrEmpty(customTenantId))
        //        {
        //            var customTenantInfo = await tenantResolverService.GetTenantInfoAsync(customTenantId);

        //            if (customTenantInfo != null)
        //            {
        //                // Overwrite the current tenant context with the custom tenant info
        //                var multiTenantContext = context.GetMultiTenantContext<TenantInfo>();
        //                if (multiTenantContext != null)
        //                {
        //                    multiTenantContext.TenantInfo = customTenantInfo;
        //                    Console.WriteLine($"Overridden tenant to: {customTenantInfo.Id}");
        //                }
        //                await dbService.InitializeApplicationDbForTenantAsync(customTenantInfo);
        //            }
        //            else
        //            {
        //                Console.WriteLine("Custom tenant not found.");
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No custom tenant ID provided.");
        //        }

        //        // Process the request with the custom tenant
        //        await next(context);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in tenant switching middleware: {ex.Message}");
        //        // Handle or log the exception as needed
        //    }
        //    finally
        //    {
        //        // Revert back to the original tenant after processing the request
        //        var multiTenantContext = context.GetMultiTenantContext<TenantInfo>();
        //        if (multiTenantContext != null)
        //        {
        //            multiTenantContext.TenantInfo = originalTenantInfo;
        //            Console.WriteLine($"Reverted back to original tenant: {originalTenantInfo?.Id ?? "No tenant"}");
        //            await dbService.InitializeApplicationDbForTenantAsync(originalTenantInfo);
        //        }
        //    }
        //});

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
