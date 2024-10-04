using Academy.Application.Batch.Dto;

namespace Academy.Application.Batch.Command.Models
{
    public class UpdateBatchRequest : IRequest<Result<BatchDto>>
    {
        public DefaultIdType Id { get; set; }
        public DefaultIdType SportsId { get; set; }
        public string BatchName { get; set; }
        public DefaultIdType CoachingId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Days { get; set; }
        public bool IsActive { get; set; }
    }
}
