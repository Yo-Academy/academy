using Academy.Application.Academies.Command.Models;
using Academy.Application.Academies.Dto;
using Academy.Application.Common.Exceptions;
using Academy.Application.Persistence.Repository;
using Mapster;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.Academies.Command.Handlers
{
    public class UpdateAcademiesRequestHandler : IRequestHandler<UpdateAcademiesRequest, Result<AcademiesDto>>
    {
        private readonly IRepository<Entites.Academies> _repository;
        public UpdateAcademiesRequestHandler(IRepository<Entites.Academies> repository)
        {
            _repository = repository;
        }
        public async Task<Result<AcademiesDto>> Handle(UpdateAcademiesRequest request, CancellationToken cancellationToken)
        {
            var AcademiesToUpdate = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (AcademiesToUpdate == null)
                return Result.Fail(new NotFoundException(DbRes.T("ErrorMessageAcademyNotFound")));

            AcademiesToUpdate.Update(request.Name,request.ShortName, request.GST, request.Address, request.City, request.Pincode);

            await _repository.UpdateAsync(AcademiesToUpdate, cancellationToken);
            return Result.Succeed(AcademiesToUpdate.Adapt<AcademiesDto>());
        }
    }
}
