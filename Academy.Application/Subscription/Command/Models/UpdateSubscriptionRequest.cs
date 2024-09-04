using Academy.Application.Subscription.Dto;

namespace Academy.Application.Subscription.Command.Models
{
    public class UpdateSubscriptionRequest : IRequest<Result<SubscriptionDto>>
    {
        public DefaultIdType Id { get; set; }
        public DefaultIdType SportsId { get; set; }
        public DefaultIdType BatchId { get; set; }
        public DefaultIdType CoachingId { get; set; }
        public DefaultIdType PlanTypeId { get; set; }
        public int Fee { get; set; }
    }
}
