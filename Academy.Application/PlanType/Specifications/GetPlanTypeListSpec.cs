using Academy.Application.PlanType.Dto;
using Academy.Application.PlanType.Query.Models;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Entity;
using Ardalis.Specification;
using Entity = Academy.Domain.Entities;


namespace Academy.Application.PlanType.Specifications
{
    public class GetPlanTypeListSpec : EntitiesByPaginationFilterSpec<Entity.PlanType, PlanTypeDto>
    {
        public GetPlanTypeListSpec(GetPlanTypeListRequest request) : base(request) => Query.OrderByDescending(x => x.CreatedOn);
    }
}
