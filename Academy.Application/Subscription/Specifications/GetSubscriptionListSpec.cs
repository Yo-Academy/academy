using Academy.Application.Subscription.Dto;
using Academy.Application.Subscription.Query.Models;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Entity;
using Ardalis.Specification;
using Entites = Academy.Domain.Entities;


namespace Academy.Application.Subscription.Specifications
{
    public class GetSubscriptionListSpec : EntitiesByPaginationFilterSpec<Entites.Subscription,SubscriptionDto>
    {
        public GetSubscriptionListSpec(GetSubscriptionListRequest request) : base(request) => Query.OrderByDescending(x => x.CreatedOn);

    }
}
