using Academy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Application.Batch.Dto
{
    public class BatchDetailsDto
    {
        public DefaultIdType Id { get; set; }
        public DefaultIdType SportsId { get; set; }
        public string BatchName { get; set; }
        public DefaultIdType CoachingId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public bool IsActive { get; set; }
        public string Days { get; set; }
        public Sports Sports { get; set; }
    }
}
