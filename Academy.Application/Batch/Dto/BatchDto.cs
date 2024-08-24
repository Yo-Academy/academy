namespace Academy.Application.Batch.Dto
{
    public class BatchDto
    {
        public DefaultIdType SportsId { get; set; }
        public string BatchName { get; set; }
        public string Coaching { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Days { get; set; }
    }
}
