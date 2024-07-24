using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Infrastructure.Mailing.AmazonSES
{
    public class AmazonSesConfig
    {
        public string DisplayName { get; set; }

        public string From { get; set; }

        public string Host { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string RegionEndpoint { get; set; }

    }
}
