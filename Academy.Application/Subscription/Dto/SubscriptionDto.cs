using Entities = Academy.Domain.Entities;

namespace Academy.Application.Subscription.Dto
{
    public class SubscriptionDto
    {
        public DefaultIdType Id { get; set; }
        public DefaultIdType SportsId { get; set; }
        public DefaultIdType BatchId { get; set; }
        public DefaultIdType CoachingId { get; set; }
        public DefaultIdType PlanTypeId { get; set; }
        public int Fee { get; set; }

        public virtual Entities.Coaching Coaching { get; set; }
        public virtual Entities.Sports Sports { get; set; }
        public virtual Entities.PlanType PlanType { get; set; }

        public virtual Entities.Batch Batch { get; set; }

    }
}
