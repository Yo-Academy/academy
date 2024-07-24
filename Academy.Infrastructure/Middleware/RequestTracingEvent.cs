using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Infrastructure.Middleware
{
    public class RequestTracingEvent
    {
        public DateTime Timestamp { get; set; }
        public string HttpMethod { get; set; }
        public string RequestUri { get; set; }
    }
}
