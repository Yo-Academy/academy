using Academy.Application.Batch.Command.Models;
using Academy.Application.Batch.Dto;
using Academy.Application.Common.Exceptions;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Mapster;
using Entities = Academy.Domain.Entities;

namespace Academy.Application.Batch.Command.Handlers
{
    public class UpdateBatchRequestHandler : IRequestHandler<UpdateBatchRequest, Result<BatchDto>>
    {
        private readonly IRepository<Entities.Batch> _repository;
        public UpdateBatchRequestHandler(IRepository<Entities.Batch> repository)
        {
            _repository = repository;
        }
        public async Task<Result<BatchDto>> Handle(UpdateBatchRequest request, CancellationToken cancellationToken)
        {
            var BatchToUpdate = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (BatchToUpdate == null)
                return Result.Fail(new NotFoundException(DbRes.T("BatchNotFound")));

            BatchToUpdate.Update(request.SportsId, request.BatchName, request.CoachingId, request.StartTime, request.EndTime, request.Days, request.IsActive);

            await _repository.UpdateAsync(BatchToUpdate, cancellationToken);
            return Result.Succeed(BatchToUpdate.Adapt<BatchDto>());
        }
    }
}
