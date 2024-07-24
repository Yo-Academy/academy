namespace Academy.Domain.Entities
{
    public class EmailHistory : AuditableEntity, IAggregateRoot
    {
        public string ToEmailAddress { get; set; } = default!;

        public string FromEmailAddress { get; set; } = default!;

        public string? CCEmailAddress { get; set; }

        public string? BCCEmailAddress { get; set; }

        public string? Subject { get; set; }

        public byte[] Body { get; set; } = default!;

        public DateTime SentOn { get; set; }

        public Guid SentBy { get; set; }

        public bool IsSent { get; set; }

    }
}
