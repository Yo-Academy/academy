using Academy.Application.Sport.Dto;
using Academy.Application.Sport.Query.Models;
using Academy.Domain.Entities;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Entity;
using Ardalis.Specification;

namespace Academy.Application.Sport.Specifications
{
    public class GetSportListSpec : EntitiesByPaginationFilterSpec<Sports, SportsDto>
    {
        public GetSportListSpec(GetSportListRequest request)
            : base(request) => Query.OrderByDescending(x => x.CreatedOn);
    }
}