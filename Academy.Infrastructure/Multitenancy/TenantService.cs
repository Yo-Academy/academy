using Academy.Application.Multitenancy;
using Academy.Infrastructure.Persistence;
using Academy.Infrastructure.Persistence.Initialization;
using Finbuckle.MultiTenant;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Academy.Infrastructure.Multitenancy
{
    internal class TenantService : ITenantService
    {
        private readonly IMultiTenantStore<TenantInfo> _tenantStore;
        private readonly IConnectionStringSecurer _csSecurer;
        private readonly IDatabaseInitializer _dbInitializer;
        private readonly DatabaseSettings _dbSettings;
        private readonly IConfiguration _config;

        public TenantService(
            IMultiTenantStore<TenantInfo> tenantStore,
            IConnectionStringSecurer csSecurer,
            IDatabaseInitializer dbInitializer,
            IOptions<DatabaseSettings> dbSettings,
            IConfiguration config)
        {
            _tenantStore = tenantStore;
            _csSecurer = csSecurer;
            _dbInitializer = dbInitializer;
            _dbSettings = dbSettings.Value;
            _config = config;
        }

        public async Task<List<TenantDto>> GetAllAsync()
        {
            var tenants = (await _tenantStore.GetAllAsync()).Adapt<List<TenantDto>>();
            tenants.ForEach(t => t.ConnectionString = _csSecurer.MakeSecure(t.ConnectionString));
            return tenants;
        }

        public async Task<bool> ExistsWithIdAsync(string id) =>
            await _tenantStore.TryGetAsync(id) is not null;

        public async Task<bool> ExistsWithNameAsync(string name) =>
            (await _tenantStore.GetAllAsync()).Any(t => t.Name == name);

        public async Task<TenantDto> GetByIdAsync(string id) =>
            (await GetTenantInfoAsync(id))
                .Adapt<TenantDto>();

        public async Task<TenantDto> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken)
        {
            if (request.ConnectionString?.Trim() == _dbSettings.ConnectionString.Trim()) request.ConnectionString = string.Empty;

            var tenant = new TenantInfo(request.Id, request.Name, request.ConnectionString, request.AdminEmail, _config, request.Phonenumber, request.Issuer);
            await _tenantStore.TryAddAsync(tenant);

            // TODO: run this in a hangfire job? will then have to send mail when it's ready or not
            try
            {
                await _dbInitializer.InitializeApplicationDbForTenantAsync(tenant, cancellationToken);
            }
            catch
            {
                await _tenantStore.TryRemoveAsync(request.Id);
                throw;
            }

            return tenant.Adapt<TenantDto>();
        }

        public async Task<TenantDto> ActivateAsync(string id)
        {
            var tenant = await GetTenantInfoAsync(id);

            if (tenant.IsActive)
            {
                throw new ConflictException(DbRes.T("TenantAlreadyActivatedMsg"));
            }

            tenant.Activate();

            await _tenantStore.TryUpdateAsync(tenant);

            return tenant.Adapt<TenantDto>();
        }

        public async Task<TenantDto> DeactivateAsync(string id)
        {
            var tenant = await GetTenantInfoAsync(id);
            if (!tenant.IsActive)
            {
                throw new ConflictException(DbRes.T("TenantAlreadyDeactivatedMsg"));
            }

            tenant.Deactivate();
            await _tenantStore.TryUpdateAsync(tenant);
            return tenant.Adapt<TenantDto>();
        }

        public async Task<TenantDto> UpdateSubscription(string id, DateTime extendedExpiryDate)
        {
            var tenant = await GetTenantInfoAsync(id);
            tenant.SetValidity(extendedExpiryDate);
            await _tenantStore.TryUpdateAsync(tenant);
            return tenant.Adapt<TenantDto>();
        }

        private async Task<TenantInfo> GetTenantInfoAsync(string id) =>
            await _tenantStore.TryGetAsync(id)
                ?? throw new NotFoundException(string.Format(DbRes.T("TenantNotFoundMsg"), typeof(TenantInfo).Name, id));
    }
}