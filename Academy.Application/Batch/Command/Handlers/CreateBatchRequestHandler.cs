using Academy.Application.Batch.Command.Models;
using Academy.Application.Batch.Dto;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Mapster;
using Entities = Academy.Domain.Entities;


namespace Academy.Application.Batch.Command.Handlers
{
    public class CreateBatchRequestHandler : IRequestHandler<CreateBatchRequest, Result<BatchDto>>
    {
        private readonly IRepository<Entities.Batch> _repository;

        public CreateBatchRequestHandler(IRepository<Entities.Batch> repository)
        {
            _repository = repository;
        }
        public async Task<Result<BatchDto>> Handle(CreateBatchRequest request, CancellationToken cancellationToken)
        {
            var id = DefaultIdType.NewGuid();
            Entities.Batch Batch = new Entities.Batch(id, request.SportsId, request.BatchName, request.CoachingId, request.StartTime, request.EndTime, request.Days);

            //Inserts RequirementSet Record
            var responseBatch = await _repository.AddAsync(Batch);


            return Result.Succeed(responseBatch.Adapt<BatchDto>());
        }
    }

}
