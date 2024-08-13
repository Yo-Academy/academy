using Academy.Infrastructure.Multitenancy;
using Academy.Shared.Multitenancy;
using Finbuckle.MultiTenant;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TenantInfo = Academy.Infrastructure.Multitenancy.TenantInfo;

namespace Academy.Infrastructure.Persistence.Initialization
{
    internal class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly TenantDbContext _tenantDbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseInitializer> _logger;
        private readonly IConfiguration _config;

        public DatabaseInitializer(TenantDbContext tenantDbContext, IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger, IConfiguration config)
        {
            _tenantDbContext = tenantDbContext;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _config = config;
        }

        public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
        {
            await InitializeTenantDbAsync(cancellationToken);

            foreach (var tenant in await _tenantDbContext.TenantInfo.ToListAsync(cancellationToken))
            {
                await InitializeApplicationDbForTenantAsync(tenant, cancellationToken);
            }
        }

        public async Task InitializeApplicationDbForTenantAsync(TenantInfo tenant, CancellationToken cancellationToken)
        {
            // First create a new scope
            using var scope = _serviceProvider.CreateScope();

            // Then set current tenant so the right connectionstring is used
            _serviceProvider.GetRequiredService<IMultiTenantContextAccessor>()
                .MultiTenantContext = new MultiTenantContext<TenantInfo>()
                {
                    TenantInfo = tenant
                };

            // Then run the initialization in the new scope
            await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>()
                .InitializeAsync(cancellationToken);
        }

        private async Task InitializeTenantDbAsync(CancellationToken cancellationToken)
        {
            if (_tenantDbContext.Database.GetPendingMigrations().Any())
            {
                _logger.LogInformation("Applying Root Migrations.");
                await _tenantDbContext.Database.MigrateAsync(cancellationToken);
            }

            await SeedRootTenantAsync(cancellationToken);
        }

        private async Task SeedRootTenantAsync(CancellationToken cancellationToken)
        {
            if (await _tenantDbContext.TenantInfo.FindAsync(new object?[] { _config.GetSection(nameof(DefaultTenantSettings)).Get<DefaultTenantSettings>()!.Id }, cancellationToken: cancellationToken) is null)
            {
                var defaultTenantSettings = _config.GetSection(nameof(DefaultTenantSettings)).Get<DefaultTenantSettings>()!;
                var rootTenant = new TenantInfo(
                    defaultTenantSettings.Id,
                    defaultTenantSettings.Name,
                    string.Empty,
                    defaultTenantSettings.EmailAddress,
                    _config, defaultTenantSettings.Phonenumber);

                rootTenant.SetValidity(DateTime.UtcNow.AddYears(10));

                _tenantDbContext.TenantInfo.Add(rootTenant);

                await _tenantDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}