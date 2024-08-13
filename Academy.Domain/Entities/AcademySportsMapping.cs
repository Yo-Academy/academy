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
        public Sports Sport { get; set; }
        public Academies Academy { get; set; }
    }
}
