namespace Academy.Application.Subscription.Dto
{
    public class SubscriptionDto
    {
        public DefaultIdType SportsId { get; set; }
        public DefaultIdType BatchId { get; set; }
        public DefaultIdType CoachingId { get; set; }
        public DefaultIdType PlanTypeId { get; set; }
        public int Fee { get; set; }
    }
}
