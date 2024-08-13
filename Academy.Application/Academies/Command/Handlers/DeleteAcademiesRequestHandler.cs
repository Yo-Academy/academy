using Academy.Application.Academies.Command.Models;
using Academy.Application.Academies.Dto;
using Academy.Application.Common.Exceptions;
using Academy.Application.Persistence.Repository;
using Mapster;
using Entites = Academy.Domain.Entities;
namespace Academy.Application.Academies.Command.Handlers
{
    public class DeleteAcademiesRequestHandler : IRequestHandler<DeleteAcademiesRequest, Result<AcademiesDto>>
    {
        private readonly IRepository<Entites.Academies> _repository;
        public DeleteAcademiesRequestHandler(IRepository<Entites.Academies> repository)
        {
            _repository = repository;
        }
        public async Task<Result<AcademiesDto>> Handle(DeleteAcademiesRequest request, CancellationToken cancellationToken)
        {
            var AcademiesToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (AcademiesToDelete == null)
                return Result.Fail(new NotFoundException("Common Lookup Not Found"));

            await _repository.DeleteAsync(AcademiesToDelete, cancellationToken);
            return Result.Succeed(AcademiesToDelete.Adapt<AcademiesDto>());
        }
    }
}
