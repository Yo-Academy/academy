using Academy.Application.Academies.Dto;
using Academy.Application.CommonLookups.Dto;
using Academy.Application.Identity.Roles.Dto;
using Academy.Domain.Entities;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;
using Entity = Academy.Domain.Entities.Academies;

namespace Academy.Application.Identity.Roles.Query.Models
{
    public class GetRolesListRequest : 
        PaginationFilter, IRequest<Result<PaginationResponse<RoleDto>>>
    {
    }
}
