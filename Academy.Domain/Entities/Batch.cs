namespace Academy.Domain.Entities
{
    public class Batch : AuditableEntity, IAggregateRoot
    {
        public DefaultIdType SportsId { get; set; }
        public string BatchName { get; set; }
        public DefaultIdType CoachingId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Days { get; set; }

        [ForeignKey("SportsId")]
        public virtual Sports Sports { get; set; }
        

        [ForeignKey("CoachingId")]
        public virtual Coaching Coaching { get; set; }
        public Batch()
        {
            
        }

        public Batch(DefaultIdType id, DefaultIdType sportsId, string batchName, DefaultIdType coachingId, TimeOnly startTime, TimeOnly endTime, string days)
        {
            Id = id;
            SportsId = sportsId;
            BatchName = batchName;
            CoachingId = coachingId;
            StartTime = startTime;
            EndTime = endTime;
            Days = days;
        }

        public Batch Update(DefaultIdType sportsId, string batchName, DefaultIdType coachingId, TimeOnly startTime, TimeOnly endTime, string days)
        {
            SportsId = sportsId;
            BatchName = batchName;
            CoachingId = coachingId;
            StartTime = startTime;
            EndTime = endTime;
            Days = days;
            return this;
        }
    }
}
