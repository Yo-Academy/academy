using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Domain.Mailing
{
    public class MailRequest
    {
        public List<string> To { get; set; }

        public string Subject { get; set; }

        public string? Body { get; set; }

        public List<string>? Bcc { get; set; }

        public List<string>? Cc { get; set; }

        public IDictionary<string, byte[]>? AttachmentData { get; set; }

    }
}
