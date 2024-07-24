using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Domain.Entities
{
    public class EmailTemplate : AuditableEntity
    {
        public string TemplateCode { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string? To { get; set; }

        public string? CC { get; set; }

        public string? BCC { get; set; }

        public string Subject { get; set; } = default!;

        public string Body { get; set; } = default!;
    }
}
