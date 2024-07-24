using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Application.EmailHistory
{
    public class EmailHistoryListDto
    {
        public string ToEmailAddress { get; set; } = default!;

        public string FromEmailAddress { get; set; } = default!;

        public string? CCEmailAddress { get; set; }

        public string? BCCEmailAddress { get; set; }

        public string? Subject { get; set; }

        public DateTime SentOn { get; set; }

        public Guid SentBy { get; set; }

        public bool IsSent { get; set; }

        public Guid Id { get; set; }
    }
}
