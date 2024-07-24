namespace Academy.Domain.Entities
{
    public class AuditLog
    {
        public DefaultIdType? Id { get; set; }
        public DefaultIdType? UserId { get; set; }
        public string? TenantId { get; set; }
        public string? Message { get; set; }
        public string? RequestPath { get; set; }
        public string? RequestMethod { get; set; }
        public string? RequestHeaders { get; set; }
        public string? RequestBody { get; set; }
        public string? Level { get; set; }
        public DateTime? ExecutionTime { get; set; }
        public long? ExecutionDuration { get; set; }
        public DefaultIdType? ErrorId { get; set; }
        public string? Exception { get; set; }
        public int? StatusCode { get; set; }
        public string? SourceContext { get; set; }
        public string? ClientIpAddress { get; set; }
        public string? BrowserInfo { get; set; }
    }
}
