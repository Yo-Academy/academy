using Academy.Application.Persistence.Repository;
using Academy.Application.Sport.Command.Models;
using Academy.Application.Sport.Dto;
using Academy.Domain.Entities;
using Mapster;

namespace Academy.Application.Sport.Command.Handlers
{
    public class CreateSportRequestHandler : IRequestHandler<CreateSportRequest, Result<SportsDto>>
    {
        private readonly IRepository<Sports> _repository;

        public CreateSportRequestHandler(IRepository<Sports> repository)
        {
            _repository = repository;
        }
        public async Task<Result<SportsDto>> Handle(CreateSportRequest request, CancellationToken cancellationToken)
        {
            var id = DefaultIdType.NewGuid();
            Sports Sport = new Sports(id, request.Name, request.Image, request.IsActive);

            //Inserts RequirementSet Record
            var responseSport = await _repository.AddAsync(Sport);

            return Result.Succeed(responseSport.Adapt<SportsDto>());
        }
    }

}
