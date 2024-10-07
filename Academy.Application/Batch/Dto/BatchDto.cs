using Academy.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Entities = Academy.Domain.Entities;

namespace Academy.Application.Batch.Dto
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
        public bool IsActive { get; set; }
        public CoachingDto Coaching { get; set; }
        public SportsDto Sports { get; set; }
    }
}
