using Ardalis.Specification;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.UserInfo.Specifications
{
    public class GetUserInfoByIdSpec : Specification<Entites.UserInfo>, ISingleResultSpecification<Entites.UserInfo>
    {
        public GetUserInfoByIdSpec(DefaultIdType id)
        {
            Query.Where(x => x.Id == id)
                   .Include(x => x.Sports)
                   .Include(x => x.Coaching)
                   .Include(x => x.Batch)
                   .Include(x => x.PlanType)
                   .Include(x => x.Subscription);
        }
    }
}
