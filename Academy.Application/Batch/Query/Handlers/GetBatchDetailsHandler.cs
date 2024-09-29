using Academy.Application.Batch.Dto;
using Academy.Application.Batch.Query.Models;
using Academy.Application.Batch.Specifications;
using Academy.Application.Contracts.Persistence;
using Academy.Application.Persistence.Repository;
using Mapster;
using Entity = Academy.Domain.Entities;


namespace Academy.Application.Batch.Query.Handlers
{
    public class GetBatchDetailsHandler : IRequestHandler<GetBatchDetailsRequest, Result<BatchDetailsDto>>
    {
        private readonly IReadRepository<Entity.Batch> _batchReadRepository;

        public GetBatchDetailsHandler(IReadRepository<Entity.Batch> batchReadRepository)
        {
            _batchReadRepository = batchReadRepository;
        }

        public async Task<Result<BatchDetailsDto>> Handle(GetBatchDetailsRequest request, CancellationToken cancellationToken)
        {
            var batchDetail = new BatchDetailsDto();
            var data = await _batchReadRepository.GetByIdAsync(request.Id, cancellationToken);
            if (data != null)
            {
                 batchDetail = data.Adapt<BatchDetailsDto>();
            }
            return Result.Succeed(batchDetail);
        }

    }


}
