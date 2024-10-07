using Academy.Application.UserInfo.Dto;
using Academy.Shared.Pagination.Models;

namespace Academy.Application.UserInfo.Query.Models
{
    public class GetUserInfoListRequest : PaginationFilter, IRequest<Result<PaginationResponse<UserInfoDto>>>
    {
    }
}
