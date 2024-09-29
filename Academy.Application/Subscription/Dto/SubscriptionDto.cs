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

        public CoachingDto Coaching { get; set; }
        public SportsDto Sports { get; set; }
        public PlanTypeDto PlanType { get; set; }
        public BatchDto Batch { get; set; }

    }
}
