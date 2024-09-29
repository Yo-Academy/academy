using Academy.Application.Coaching.Dto;
using Academy.Shared.Pagination.Models;

namespace Academy.Application.Coaching.Query.Models
{
    public class GetCoachingListRequest : PaginationFilter, IRequest<Result<PaginationResponse<CoachingDto>>>
    {

    }
}
