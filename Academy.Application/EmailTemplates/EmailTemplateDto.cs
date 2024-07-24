using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Application.EmailTemplates
{
    public class EmailTemplateDto
    {
        public string TemplateCode { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string? To { get; set; }

        public string? CC { get; set; }

        public string? BCC { get; set; }

        public string Subject { get; set; } = default!;

        public string Body { get; set; } = default!;

        public Guid Id { get; set; }
    }
}
