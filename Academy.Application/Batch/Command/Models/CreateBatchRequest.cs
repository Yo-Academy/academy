using Academy.Application.Batch.Command.Validators;
using Academy.Application.Batch.Dto;

namespace Academy.Application.Batch.Command.Models
{
    public class CreateBatchRequest : IRequest<Result<BatchDto>>
    {
        public DefaultIdType SportsId { get; set; }
        public string BatchName { get; set; }
        public DefaultIdType CoachingId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Days { get; set; }
        public  bool  IsActive { get; set; }
    }
}
