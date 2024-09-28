using Academy.Application.Coaching.Dto;
using Academy.Application.Coaching.Query.Models;
using Academy.Shared.Pagination;
using Ardalis.Specification;
using Entity = Academy.Domain.Entities;

namespace Academy.Application.Coaching.Specifications
{
    public class GetCoachingListSpec : EntitiesByPaginationFilterSpec<Entity.Coaching, CoachingDto>
    {
        public GetCoachingListSpec(GetCoachingListRequest request) : base(request) => Query.OrderByDescending(x => x.CreatedOn);
    }
}
