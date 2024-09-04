using Academy.Application.Batch.Dto;
using Academy.Application.Batch.Query.Models;
using Academy.Shared.Pagination;
using Ardalis.Specification;
using Entity = Academy.Domain.Entities;


namespace Academy.Application.Batch.Specifications
{
    public class GetBatchListSpec : EntitiesByPaginationFilterSpec<Entity.Batch, BatchDto>
    {
        public GetBatchListSpec(GetBatchListRequest request) : base(request) => Query.OrderByDescending(x => x.CreatedOn);
    }
}
