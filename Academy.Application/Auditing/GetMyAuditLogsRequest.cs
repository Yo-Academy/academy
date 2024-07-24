namespace Academy.Application.Auditing
{
    public class GetMyAuditLogsRequest : IRequest<Result<List<AuditDto>>>
    {
    }

    public class GetMyAuditLogsRequestHandler : IRequestHandler<GetMyAuditLogsRequest, Result<List<AuditDto>>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IAuditService _auditService;

        public GetMyAuditLogsRequestHandler(ICurrentUser currentUser, IAuditService auditService) =>
            (_currentUser, _auditService) = (currentUser, auditService);

        public async Task<Result<List<AuditDto>>> Handle(GetMyAuditLogsRequest request, CancellationToken cancellationToken) =>
            Result.Succeed(await _auditService.GetUserTrailsAsync(_currentUser.GetUserId()));
    }
}