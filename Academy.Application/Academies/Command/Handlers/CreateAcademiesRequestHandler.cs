using Academy.Application.Academies.Command.Models;
using Academy.Application.Academies.Dto;
using Academy.Application.Persistence.Repository;
using Mapster;
using Entities = Academy.Domain.Entities;

namespace Academy.Application.Academies.Command.Handlers
{
    public class CreateAcademiesRequestHandler : IRequestHandler<CreateAcademiesRequest, Result<AcademiesDto>>
    {
        private readonly IRepository<Entities.Academies> _repository;

        public CreateAcademiesRequestHandler(IRepository<Entities.Academies> repository)
        {
            _repository = repository;
        }
        public async Task<Result<AcademiesDto>> Handle(CreateAcademiesRequest request, CancellationToken cancellationToken)
        {
            var id = DefaultIdType.NewGuid();
            Entities.Academies Academies = new Entities.Academies(id, request.Name, request.ShortName, request.AcademyId,
                request.GST, request.Address, request.City, request.Pincode);

            //Inserts RequirementSet Record
            var responseAcademies = await _repository.AddAsync(Academies);

            return Result.Succeed(responseAcademies.Adapt<AcademiesDto>());
        }
    }

}
