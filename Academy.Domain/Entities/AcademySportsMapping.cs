using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Domain.Entities
{
    public class AcademySportsMapping : AuditableEntity, IAggregateRoot
    {
        public DefaultIdType AcademyId { get; set; }
        public DefaultIdType SportId { get; set; }

        public AcademySportsMapping(DefaultIdType id, DefaultIdType academyId, DefaultIdType sportId)
        {
            Id = id;
            AcademyId = academyId;
            SportId = sportId;
        }

        public AcademySportsMapping Update(DefaultIdType academyId, DefaultIdType sportId)
        {
            AcademyId = AcademyId;
            SportId = sportId;
            return this;
        }
    }
}
