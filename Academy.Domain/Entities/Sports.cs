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
        [ForeignKey("SportId")]
        public List<AcademySportsMapping> AcademySportsMapping { get; set; }
        public Sports() { }

        public Sports(DefaultIdType id, string name, string image, bool isActive)
        {
            Id = id;
            Name = name;
            Image = image;
            IsActive = isActive;
        }

        public Sports Update(string name, string image, bool isActive)
        {
            Name = name;
            Image = image;
            IsActive = isActive;
            return this;
        }
    }
}
