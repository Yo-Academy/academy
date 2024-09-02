using Academy.Application.Batch.Command.Models;
using Academy.Application.Batch.Dto;
using Academy.Application.Common.Exceptions;
using Academy.Application.Persistence.Repository;
using Mapster;
using Entities = Academy.Domain.Entities;


namespace Academy.Application.Batch.Command.Handlers
{
    public class DeleteBatchRequestHandler : IRequestHandler<DeleteBatchRequest, Result<BatchDto>>
    {
        private readonly IRepository<Entities.Batch> _repository;
        public DeleteBatchRequestHandler(IRepository<Entities.Batch> repository)
        {
            _repository = repository;
        }
        public async Task<Result<BatchDto>> Handle(DeleteBatchRequest request, CancellationToken cancellationToken)
        {
            var BatchToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (BatchToDelete == null)
                return Result.Fail(new NotFoundException(DbRes.T("BatchNotFound")));
            

            await _repository.DeleteAsync(BatchToDelete, cancellationToken);
            return Result.Succeed(BatchToDelete.Adapt<BatchDto>());
        }
    }
}
