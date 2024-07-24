using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Infrastructure.Mailing.SendGrid
{
    public class SendGridConfig
    {
        public string ApiKey { get; set; }

        public string DisplayName { get; set; }

        public string From { get; set; }
    }
}
