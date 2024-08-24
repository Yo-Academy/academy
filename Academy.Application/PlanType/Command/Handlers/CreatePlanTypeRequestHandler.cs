using Academy.Application.Persistence.Repository;
using Academy.Application.PlanType.Command.Models;
using Academy.Application.PlanType.Dto;
using Mapster;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.PlanType.Command.Handlers
{
    public class CreatePlanTypeRequestHandler : IRequestHandler<CreatePlanTypeRequest, Result<PlanTypeDto>>
    {
        private readonly IRepository<Entites.PlanType> _repository;

        public CreatePlanTypeRequestHandler(IRepository<Entites.PlanType> repository)
        {
            _repository = repository;
        }
        public async Task<Result<PlanTypeDto>> Handle(CreatePlanTypeRequest request, CancellationToken cancellationToken)
        {
            var id = DefaultIdType.NewGuid();
            Entites.PlanType PlanType = new Entites.PlanType(id, request.Code, request.Name);

            //Inserts RequirementSet Record
            var responsePlanType = await _repository.AddAsync(PlanType);

            return Result.Succeed(responsePlanType.Adapt<PlanTypeDto>());
        }
    }

}
