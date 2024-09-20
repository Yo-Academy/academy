using Academy.Application.Academies.Dto;
using Academy.Application.Academies.Query.Models;
using Academy.Application.Identity.Roles.Dto;
using Academy.Application.Identity.Roles.Query.Models;
using Academy.Domain.Identity;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Entity;
using Ardalis.Specification;
using Entity = Academy.Domain.Entities;

namespace Academy.Application.Identity.Roles.Specifications
{
    public class GetRolesListSpec : EntitiesByPaginationFilterSpec<ApplicationRole, RoleDto>
    {
        public GetRolesListSpec(GetRolesListRequest request)
            : base(request) => Query.OrderByDescending(x => x.CreatedOn);
    }
}
