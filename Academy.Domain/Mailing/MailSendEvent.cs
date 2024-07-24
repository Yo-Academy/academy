using Academy.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Domain.Mailing
{
    public class MailSendEvent : DomainEvent
    {
        public MailRequest MailRequest { get; set; }
        public MailSendEvent(MailRequest mailRequest)
        {
            MailRequest = mailRequest;
        }
    }
}
