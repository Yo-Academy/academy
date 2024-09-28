using Academy.Application.Academies.Dto;
using Academy.Shared.Pagination.Models;

namespace Academy.Application.Identity.Users.Query.Models
{
    public class GetUsersListRequest : PaginationFilter, IRequest<Result<PaginationResponse<UserDetailsDto>>>
    {

    }
}
