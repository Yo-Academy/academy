using Academy.Application.Academies.Query.Models;
using Academy.Application.Identity.Users.Query.Models;
using Academy.Domain.Identity;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Entity;
using Ardalis.Specification;

namespace Academy.Application.Identity.Users.Specifications
{
    public class GetByTenantIdSpec : Specification<ApplicationUser>
    {
        public GetByTenantIdSpec(string? tenantId)
        {
            Query.Where(x => x.TenantId.Equals(tenantId));
        }
    }
}
