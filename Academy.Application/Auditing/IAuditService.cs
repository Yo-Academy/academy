namespace Academy.Application.Auditing
{
    public interface IAuditService : ITransientService
    {
        Task<List<AuditDto>> GetUserTrailsAsync(DefaultIdType userId);
    }
}