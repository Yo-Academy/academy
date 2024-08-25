namespace Academy.Domain.Entities
{
    public class Subscription: AuditableEntity, IAggregateRoot
    {
        public DefaultIdType SportsId { get; set; }
        public DefaultIdType BatchId { get; set; }
        public DefaultIdType CoachingId { get; set; }
        public DefaultIdType PlanTypeId { get; set; }
        public int Fee { get; set; }

        [ForeignKey("SportsId")]
        public virtual Sports  Sports{ get; set; }

        [ForeignKey("BatchId")]
        public virtual Batch Batch { get; set; }

        [ForeignKey("CoachingId")]
        public virtual Coaching Coaching { get; set; }

        [ForeignKey("PlanTypeId")]
        public virtual PlanType PlanType { get; set; }

        public Subscription()
        {
            
        }

        public Subscription(DefaultIdType id, DefaultIdType sportsId, DefaultIdType batchId, DefaultIdType coachingId, DefaultIdType planTypeId, int fee)
        {
            Id = id;
            SportsId = sportsId;
            BatchId = batchId;
            CoachingId = coachingId;
            PlanTypeId = planTypeId;
            Fee = fee;
        }

        public Subscription Update(DefaultIdType sportsId, DefaultIdType batchId, DefaultIdType coachingId, DefaultIdType planTypeId, int fee)
        {
            SportsId = sportsId;
            BatchId = batchId;
            CoachingId = coachingId;
            PlanTypeId = planTypeId;
            Fee = fee;
            return this;
        }
    }
}
