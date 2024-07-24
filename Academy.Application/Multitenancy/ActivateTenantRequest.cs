namespace Academy.Application.Multitenancy
{
    public class ActivateTenantRequest : IRequest<Result<TenantDto>>
    {
        public string TenantId { get; set; } = default!;

        public ActivateTenantRequest(string tenantId) => TenantId = tenantId;
    }

    public class ActivateTenantRequestValidator : CustomValidator<ActivateTenantRequest>
    {
        public ActivateTenantRequestValidator() =>
            RuleFor(t => t.TenantId)
                .NotEmpty();
    }

    public class ActivateTenantRequestHandler : IRequestHandler<ActivateTenantRequest, Result<TenantDto>>
    {
        private readonly ITenantService _tenantService;

        public ActivateTenantRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

        public async Task<Result<TenantDto>> Handle(ActivateTenantRequest request, CancellationToken cancellationToken) =>
            Result.Succeed(await _tenantService.ActivateAsync(request.TenantId));
    }
}