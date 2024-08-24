using Academy.Application.Coaching.Command.Models;
using Academy.Application.Coaching.Dto;
using Academy.Application.Persistence.Repository;
using Mapster;
using Entities = Academy.Domain.Entities;


namespace Academy.Application.Coaching.Command.Handlers
{
    public class CreateCoachingRequestHandler : IRequestHandler<CreateCoachingRequest, Result<CoachingDto>>
    {
        private readonly IRepository<Entities.Coaching> _repository;

        public CreateCoachingRequestHandler(IRepository<Entities.Coaching> repository)
        {
            _repository = repository;
        }
        public async Task<Result<CoachingDto>> Handle(CreateCoachingRequest request, CancellationToken cancellationToken)
        {
            var id = DefaultIdType.NewGuid();
            Entities.Coaching Coaching = new Entities.Coaching(id, request.Code, request.Name);

            //Inserts RequirementSet Record
            var responseCoaching = await _repository.AddAsync(Coaching);

            return Result.Succeed(responseCoaching.Adapt<CoachingDto>());
        }
    }

}
