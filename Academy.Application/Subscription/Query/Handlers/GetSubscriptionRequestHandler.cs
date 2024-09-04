using Academy.Application.Persistence.Repository;
using Academy.Application.Subscription.Dto;
using Academy.Application.Subscription.Query.Models;
using Academy.Application.Subscription.Specifications;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;
using Mapster;
using Entites = Academy.Domain.Entities;


namespace Academy.Application.Subscription.Query.Handlers
{
    public class GetSubscriptionListRequestHandler : IRequestHandler<GetSubscriptionListRequest, Result<PaginationResponse<SubscriptionDto>>>
    {
        private readonly IReadRepository<Entites.Subscription> _repository;
        public GetSubscriptionListRequestHandler(IReadRepository<Entites.Subscription> repository)
        {
            _repository = repository;
        }
        public async Task<Result<PaginationResponse<SubscriptionDto>>> Handle(GetSubscriptionListRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetSubscriptionListSpec(request);
            var data = await _repository.PaginatedListAsync(spec,
                                                           request.PageNumber,
                                                           request.PageSize,
                                                           cancellationToken);

            if (data.Data != null && data.Data.Count > 0)
            {
                var subscriptionList = data.Data.Adapt<List<SubscriptionDto>>();
                data.Data = subscriptionList;
            }
            return Result.Succeed(data);
        }
    }
}
