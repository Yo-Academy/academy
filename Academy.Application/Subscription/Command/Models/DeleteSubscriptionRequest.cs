using Academy.Application.Subscription.Dto;

namespace Academy.Application.Subscription.Command.Models
{
    public class DeleteSubscriptionRequest : IRequest<Result<SubscriptionDto>>
    {
        public DefaultIdType Id { get; set; }
    }
}
