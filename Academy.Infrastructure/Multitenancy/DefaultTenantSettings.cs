using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Infrastructure.Multitenancy
{
    public class DefaultTenantSettings
    {
        public string Id { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }
    }
}
