using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Domain.Entities
{
    public class Sports : AuditableEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public List<AcademySportsMapping> AcademySportsMapping { get; set; }
    }
}
