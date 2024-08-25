using Academy.Application.Subscription.Dto;
using Academy.Shared.Pagination.Models;

namespace Academy.Application.Subscription.Query.Models
{
    public class GetSubscriptionListRequest : PaginationFilter, IRequest<Result<PaginationResponse<SubscriptionDto>>>
    {
    }
}
