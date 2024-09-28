using Academy.Application.Batch.Dto;
using Academy.Application.Batch.Query.Models;
using Academy.Application.Batch.Specifications;
using Academy.Application.Persistence.Repository;
using Academy.Application.Subscription.Dto;
using Academy.Application.Subscription.Query.Models;
using Academy.Application.Subscription.Specifications;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var spec = new GetSubscriptionByIdSpec(request.Id);
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
