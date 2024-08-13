using Academy.Application.Academies.Dto;
using Academy.Application.Academies.Query.Models;
using Academy.Shared.Pagination;
using Ardalis.Specification;
using Entity = Academy.Domain.Entities;

namespace Academy.Application.Academies.Specifications
{
    public class GetAcademiesListSpec : EntitiesByPaginationFilterSpec<Entity.Academies, AcademiesDto>
    {
        public GetAcademiesListSpec(GetAcademiesListRequest request)
            : base(request) => Query.OrderByDescending(x => x.CreatedOn);
    }
}
