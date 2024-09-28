using Academy.Infrastructure.Persistence;
using Academy.Infrastructure.Persistence.Initialization;
using Academy.Shared;
using Academy.Shared.Multitenancy;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Threading;

namespace Academy.Infrastructure.Multitenancy
{
    // TenantResolverService implements the logic for resolving the tenant
    internal class TenantResolverService : ITenantResolverService
    {
        private readonly IMultiTenantStore<TenantInfo> _tenantStore;
        private readonly ITenantInfo? _currentTenant;

        // Define unique object keys for tenant tracking
        private static readonly object PreviousTenantKey = new();
        private static readonly object CurrentTenantKey = new();
        private readonly IDatabaseInitializer _dbInitializer;
        private readonly DatabaseSettings _dbSettings;


        public TenantResolverService(IMultiTenantStore<TenantInfo> tenantStore,
            IDatabaseInitializer dbInitializer,
            ITenantInfo? currentTenant,
            IOptions<DatabaseSettings> dbSettings
            )
        {
            _tenantStore = tenantStore;
            _currentTenant = currentTenant;
            _dbInitializer = dbInitializer;
            _dbSettings = dbSettings.Value;
        }

        public async Task<string?> ResolveTenantIdAsync(HttpContext context)
        {
            // Resolve the tenant based on claims, headers, or query string
            context.Request.Headers.TryGetValue(MultitenancyConstants.TenantIdName, out StringValues tenantId);
            if (string.IsNullOrEmpty(tenantId))
            {
                context.Request.Query.TryGetValue(MultitenancyConstants.TenantIdName, out tenantId);
            }

            return await Task.FromResult(tenantId.ToString());
        }

        public async Task<TenantInfo?> GetTenantInfoAsync(string tenantId)
        {
            return await _tenantStore.TryGetAsync(tenantId);
        }

        public async Task SwitchTenantAsync(HttpContext context, string tenantId, CancellationToken cancellationToken)
        {
            var currentTenantInfo = context.GetMultiTenantContext<TenantInfo>()?.TenantInfo;

            // Store the current tenant as the previous tenant
            if (currentTenantInfo != null)
            {
                context.Items[PreviousTenantKey] = currentTenantInfo;
            }

            // Switch to the new tenant
            var newTenantInfo = await GetTenantInfoAsync(tenantId);
            if (newTenantInfo != null)
            {
                var multiTenantContext = context.GetMultiTenantContext<TenantInfo>();
                if (multiTenantContext != null)
                {
                    multiTenantContext.TenantInfo = newTenantInfo;
                    context.Items[CurrentTenantKey] = newTenantInfo;

                    await _dbInitializer.InitializeApplicationDbForTenantAsync(newTenantInfo, cancellationToken);
                }
            }
        }

        public async Task RevertToPreviousTenantAsync(HttpContext context, CancellationToken cancellationToken)
        {
            // Retrieve the previous tenant info from the context
            if (context.Items.ContainsKey(PreviousTenantKey))
            {
                var previousTenantInfo = context.Items[PreviousTenantKey] as TenantInfo;
                if (previousTenantInfo != null)
                {
                    var multiTenantContext = context.GetMultiTenantContext<TenantInfo>();
                    if (multiTenantContext != null)
                    {
                        multiTenantContext.TenantInfo = previousTenantInfo;
                        context.Items[CurrentTenantKey] = previousTenantInfo;

                        await _dbInitializer.InitializeApplicationDbForTenantAsync(previousTenantInfo, cancellationToken);
                    }
                }
            }
        }
    }
}
