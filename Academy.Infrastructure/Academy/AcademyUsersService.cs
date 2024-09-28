using Academy.Application.Academies.Contracts;
using Academy.Application.Identity.Users;
using Academy.Application.Multitenancy;
using Finbuckle.MultiTenant;

namespace Academy.Infrastructure.Identity
{
    internal class AcademyUsersService : IAcademyUsersService
    {
        private readonly ITenantService _tenantService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMultiTenantStore<TenantInfo> _tenantStore;
        public AcademyUsersService(ITenantService tenantService,
            IServiceProvider serviceProvider,
            IMultiTenantStore<TenantInfo> tenantStore)
        {
            _tenantService = tenantService;
            _serviceProvider = serviceProvider;
            _tenantStore = tenantStore;
        }

        public async Task<Result<List<UserDetailsDto>>> GetUsersAsync(string tenantId, CancellationToken cancellationToken)
        {
            var tenant = await _tenantStore.TryGetAsync(tenantId);

            // TODO: run this in a hangfire job? will then have to send mail when it's ready or not
            try
            {
                if (tenant != null)
                {
                    // First create a new scope
                    using var scope = _serviceProvider.CreateScope();

                    // Then set current tenant so the right connectionstring is used
                    _serviceProvider.GetRequiredService<IMultiTenantContextAccessor>()
                        .MultiTenantContext = new MultiTenantContext<TenantInfo>()
                        {
                            TenantInfo = tenant.Adapt<TenantInfo>()
                        };

                    // Then run the initialization in the new scope
                    return await scope.ServiceProvider.GetRequiredService<IUserService>()
                        .GetListAsync(cancellationToken);
                }
                else
                {
                    return Result.Fail();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
