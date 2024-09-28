using Academy.Application.Academies.Command.Models;
using Academy.Application.Multitenancy;
using Academy.Infrastructure.Multitenancy;

namespace Academy.Infrastructure.Persistence.Initialization
{
    internal interface IDatabaseInitializer
    {
        Task InitializeDatabasesAsync(CancellationToken cancellationToken);
        Task InitializeApplicationDbForTenantAsync(TenantInfo tenant, CancellationToken cancellationToken = default!);
        Task InitializeApplicationDbForTenantWithUsersAsync(TenantInfo tenant, CreateAcademyUserRequest request, CancellationToken cancellationToken = default!);
    }
}