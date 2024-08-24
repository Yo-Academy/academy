using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Academy.Domain.Entities
{
    public class Batch : AuditableEntity, IAggregateRoot
    {
        public DefaultIdType SportsId { get; set; }
        public string BatchName { get; set; }
        public string Coaching { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Days { get; set; }

        [ForeignKey("SportsId")]
        public virtual Sports Sports { get; set; }

        public Batch()
        {
            
        }

        public Batch(DefaultIdType id, DefaultIdType sportsId, string batchName, string coaching, TimeOnly startTime, TimeOnly endTime, string days)
        {
            Id = id;
            SportsId = sportsId;
            BatchName = batchName;
            Coaching = coaching;
            StartTime = startTime;
            EndTime = endTime;
            Days = days;
        }

        public Batch Update(DefaultIdType sportsId, string batchName, string coaching, TimeOnly startTime, TimeOnly endTime, string days)
        {
            SportsId = sportsId;
            BatchName = batchName;
            Coaching = coaching;
            StartTime = startTime;
            EndTime = endTime;
            Days = days;
            return this;
        }
    }
}
