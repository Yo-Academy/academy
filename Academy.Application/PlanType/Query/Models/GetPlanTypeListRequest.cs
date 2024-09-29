using Academy.Application.PlanType.Dto;
using Academy.Shared.Pagination.Models;

namespace Academy.Application.PlanType.Query.Models
{
    public class GetPlanTypeListRequest : PaginationFilter, IRequest<Result<PaginationResponse<PlanTypeDto>>>
    {
    }
}
