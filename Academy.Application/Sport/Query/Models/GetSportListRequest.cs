using Academy.Application.Academies.Dto;
using Academy.Application.Sport.Dto;
using Academy.Shared.Pagination.Models;

namespace Academy.Application.Sport.Query.Models
{
    public class GetSportListRequest :
        PaginationFilter, IRequest<Result<PaginationResponse<SportsDto>>>
    {
    }
}
