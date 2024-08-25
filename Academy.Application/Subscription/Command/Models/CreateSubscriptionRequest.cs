using Academy.Application.Subscription.Command.Validators;
using Academy.Application.Subscription.Dto;

namespace Academy.Application.Subscription.Command.Models
{
    public class CreateSubscriptionRequest : IRequest<Result<SubscriptionDto>>
    {
        public DefaultIdType SportsId { get; set; }
        public DefaultIdType BatchId { get; set; }
        public DefaultIdType CoachingId { get; set; }
        public DefaultIdType PlanTypeId { get; set; }
        public int Fee { get; set; }
    }
}
