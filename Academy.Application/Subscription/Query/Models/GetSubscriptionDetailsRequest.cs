using Academy.Application.Subscription.Dto;

namespace Academy.Application.Subscription.Query.Models
{
    public class GetSubscriptionDetailsRequest : IRequest<Result<SubscriptionDto>>
    {
        public DefaultIdType Id { get; set; }
    }
}
