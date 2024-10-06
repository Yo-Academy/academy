using Academy.Application.Persistence.Repository;
using Academy.Application.Subscription.Dto;
using Academy.Application.Subscription.Query.Models;
using Mapster;
using Entity = Academy.Domain.Entities;


namespace Academy.Application.Subscription.Query.Handlers
{
    public class GetSubscriptionDetailsHandler : IRequestHandler<GetSubscriptionDetailsRequest, Result<SubscriptionDto>>
    {
        private readonly IReadRepository<Entity.Subscription> _subscriptionReadRepository;

        public GetSubscriptionDetailsHandler(IReadRepository<Entity.Subscription> subscriptionReadRepository)
        {
            _subscriptionReadRepository = subscriptionReadRepository;
        }

        public async Task<Result<SubscriptionDto>> Handle(GetSubscriptionDetailsRequest request, CancellationToken cancellationToken)
        {
            var subscriptionDetail = new SubscriptionDto();
            var data = await _subscriptionReadRepository.GetByIdAsync(request.Id, cancellationToken);
            if (data != null)
            {
                subscriptionDetail = data.Adapt<SubscriptionDto>();
            }
            return Result.Succeed(subscriptionDetail);
        }
    }
}
