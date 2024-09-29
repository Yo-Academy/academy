namespace Academy.Application.Subscription.Dto
{
    public class BatchDto
    {
        public DefaultIdType Id { get; set; }
        public DefaultIdType SportsId { get; set; }
        public string BatchName { get; set; }
        public DefaultIdType CoachingId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Days { get; set; }
    }
}
