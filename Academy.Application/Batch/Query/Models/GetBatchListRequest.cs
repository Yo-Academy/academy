using Academy.Application.Batch.Dto;
using Academy.Shared.Pagination.Models;

namespace Academy.Application.Batch.Query.Models
{
    public class GetBatchListRequest : PaginationFilter, IRequest<Result<PaginationResponse<BatchDto>>>
    {
    }
}
