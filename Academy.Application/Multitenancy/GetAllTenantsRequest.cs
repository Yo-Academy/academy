namespace Academy.Application.Multitenancy
{
    public class GetAllTenantsRequest : IRequest<Result<List<TenantDto>>>
    {
    }

    public class GetAllTenantsRequestHandler : IRequestHandler<GetAllTenantsRequest, Result<List<TenantDto>>>
    {
        private readonly ITenantService _tenantService;

        public GetAllTenantsRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

        public async Task<Result<List<TenantDto>>> Handle(GetAllTenantsRequest request, CancellationToken cancellationToken) =>
            Result.Succeed(await _tenantService.GetAllAsync());
    }
}