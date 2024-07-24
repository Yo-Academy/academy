namespace Academy.Infrastructure.Middleware
{
    [Serializable]
    public class AuditLogs
    {
        public DateTime? ExecutionTime { get; set; }
        public long? ExecutionDuration { get; set; }
        public DefaultIdType? ErrorId { get; set; }
        public int? StatusCode { get; set; }
        public string? RequestHeaders { get; set; }
        public string? RequestBody { get; set; }
    }
}