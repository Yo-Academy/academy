namespace Academy.Application.Multitenancy
{
    public class GetTenantRequest : IRequest<Result<TenantDto>>
    {
        public string TenantId { get; set; } = default!;

        public GetTenantRequest(string tenantId) => TenantId = tenantId;
    }

    public class GetTenantRequestValidator : CustomValidator<GetTenantRequest>
    {
        public GetTenantRequestValidator() =>
            RuleFor(t => t.TenantId)
                .NotEmpty();
    }

    public class GetTenantRequestHandler : IRequestHandler<GetTenantRequest, Result<TenantDto>>
    {
        private readonly ITenantService _tenantService;

        public GetTenantRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

        public async Task<Result<TenantDto>> Handle(GetTenantRequest request, CancellationToken cancellationToken) =>
            Result.Succeed(await _tenantService.GetByIdAsync(request.TenantId));
    }
}