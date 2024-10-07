using Ardalis.Specification;
using Entites = Academy.Domain.Entities;


namespace Academy.Application.Subscription.Specifications
{
    public class GetSubscriptionByIdSpec : Specification<Entites.Subscription>, ISingleResultSpecification<Entites.Subscription>
    {
        public GetSubscriptionByIdSpec(DefaultIdType id)
        {
            Query.Where(x => x.Id == id)
                   .Include(x => x.Sports)
                   .Include(x => x.Coaching)
                   .Include(x => x.Batch)
                   .Include(x =>x.PlanType);
        }
    }
}
