using Academy.Application.UserPaymentInfo.Dto;
using Academy.Shared.Pagination.Models;


namespace Academy.Application.UserPaymentInfo.Query.Models
{
    public class GetUserPaymentInfoListRequest : PaginationFilter, IRequest<Result<PaginationResponse<UserPaymentInfoDto>>>
    {  
    }
}
