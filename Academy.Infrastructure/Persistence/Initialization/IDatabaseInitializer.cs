using Academy.Infrastructure.Multitenancy;

namespace Academy.Infrastructure.Persistence.Initialization
{
    internal interface IDatabaseInitializer
    {
        Task InitializeDatabasesAsync(CancellationToken cancellationToken);
        Task InitializeApplicationDbForTenantAsync(TenantInfo tenant, CancellationToken cancellationToken = default!);
    }
}