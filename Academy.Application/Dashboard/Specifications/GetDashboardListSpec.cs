using Academy.Application.Dashboard.Query.Models;
using Ardalis.Specification;
using Entity = Academy.Domain.Entities;

namespace Academy.Application.Dashboard.Specifications
{
    public class GetDashboardListSpec : Specification<Entity.Academies>, ISingleResultSpecification<Entity.Academies>
    {
        public GetDashboardListSpec(GetDashboardListRequest request)
        {
            Query.Where(x => x.Id == request.Id).OrderByDescending(x => x.CreatedOn).Include(x => x.AcademySportsMappings);
        }
    }
}
