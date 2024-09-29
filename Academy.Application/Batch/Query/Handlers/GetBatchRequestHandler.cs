using Academy.Application.Batch.Dto;
using Academy.Application.Batch.Query.Models;
using Academy.Application.Batch.Specifications;
using Academy.Application.Contracts.Persistence;
using Academy.Application.Persistence.Repository;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;
using Mapster;
using Entity = Academy.Domain.Entities.Batch;


namespace Academy.Application.Batch.Query.Handlers
{
    public class GetBatchListRequestHandler : IRequestHandler<GetBatchListRequest, Result<PaginationResponse<BatchDto>>>
    {
        private readonly IReadRepository<Entity> _repository;
       // private readonly IBatchRepository _batchRepository;
        public GetBatchListRequestHandler(IReadRepository<Entity> repository)//, IBatchRepository batchRepository)
        {
            _repository = repository;
            //_batchRepository = batchRepository;
        }
        public async Task<Result<PaginationResponse<BatchDto>>> Handle(GetBatchListRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetBatchListSpec(request);
            var data = await _repository.PaginatedListAsync(spec,
                                                            request.PageNumber,
                                                            request.PageSize,
                                                            cancellationToken);

            if (data.Data != null && data.Data.Count > 0)
            {
                var batchList = data.Data.Adapt<List<BatchDto>>();
                data.Data = batchList;
            }
            return Result.Succeed(data);
        }
    }
}
