using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Infrastructure.Middleware
{
    public class ResponseTracingEvent : RequestTracingEvent
    {
        public int HttpStatusCode { get; set; }
        public long DurationMillseconds { get; set; }
    }
}
