namespace Academy.Application.Multitenancy
{
    public class DeactivateTenantRequest : IRequest<Result<TenantDto>>
    {
        public string TenantId { get; set; } = default!;

        public DeactivateTenantRequest(string tenantId) => TenantId = tenantId;
    }

    public class DeactivateTenantRequestValidator : CustomValidator<DeactivateTenantRequest>
    {
        public DeactivateTenantRequestValidator() =>
            RuleFor(t => t.TenantId)
                .NotEmpty();
    }

    public class DeactivateTenantRequestHandler : IRequestHandler<DeactivateTenantRequest, Result<TenantDto>>
    {
        private readonly ITenantService _tenantService;

        public DeactivateTenantRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

        public async Task<Result<TenantDto>> Handle(DeactivateTenantRequest request, CancellationToken cancellationToken) =>
            Result.Succeed(await _tenantService.DeactivateAsync(request.TenantId));
    }
}