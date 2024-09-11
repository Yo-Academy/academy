using Academy.Application.Academies.Dto;
using Academy.Application.Permission.Dto;
using Academy.Shared.Pagination.Models;

namespace Academy.Application.Permission.Query.Models
{
    public class GetPermissionListRequest :
        PaginationFilter, IRequest<Result<PaginationResponse<PermissionDto>>>
    {
    }
}
