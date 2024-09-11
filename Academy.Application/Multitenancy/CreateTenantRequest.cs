namespace Academy.Application.Multitenancy
{
    public class CreateTenantRequest : IRequest<Result<TenantDto>>
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? ConnectionString { get; set; }
        public string AdminEmail { get; set; } = default!;
        public string? Issuer { get; set; }
    }

    public class CreateTenantRequestHandler : IRequestHandler<CreateTenantRequest, Result<TenantDto>>
    {
        private readonly ITenantService _tenantService;

        public CreateTenantRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

        public async Task<Result<TenantDto>> Handle(CreateTenantRequest request, CancellationToken cancellationToken) =>
            Result.Succeed(await _tenantService.CreateAsync(request, cancellationToken));
    }
}