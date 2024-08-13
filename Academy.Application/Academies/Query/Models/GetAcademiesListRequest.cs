using Academy.Application.Academies.Dto;
using Academy.Application.CommonLookups.Dto;
using Academy.Domain.Entities;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;
using Entity = Academy.Domain.Entities.Academies;

namespace Academy.Application.Academies.Query.Models
{
    public class GetAcademiesListRequest : 
        PaginationFilter, IRequest<Result<PaginationResponse<AcademiesDto>>>
    {
    }
}
