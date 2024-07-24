namespace Academy.Application.Multitenancy
{
    public class UpgradeSubscriptionRequest : IRequest<Result<TenantDto>>
    {
        public string TenantId { get; set; } = default!;
        public DateTime ExtendedExpiryDate { get; set; }
    }

    public class UpgradeSubscriptionRequestValidator : CustomValidator<UpgradeSubscriptionRequest>
    {
        public UpgradeSubscriptionRequestValidator() =>
            RuleFor(t => t.TenantId)
                .NotEmpty();
    }

    public class UpgradeSubscriptionRequestHandler : IRequestHandler<UpgradeSubscriptionRequest, Result<TenantDto>>
    {
        private readonly ITenantService _tenantService;

        public UpgradeSubscriptionRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

        public async Task<Result<TenantDto>> Handle(UpgradeSubscriptionRequest request, CancellationToken cancellationToken) =>
             Result.Succeed(await _tenantService.UpdateSubscription(request.TenantId, request.ExtendedExpiryDate));
    }
}