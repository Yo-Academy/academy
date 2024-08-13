using Academy.Application.Common.Exceptions;
using Academy.Application.Persistence.Repository;
using Academy.Application.Sport.Command.Models;
using Academy.Application.Sport.Dto;
using Academy.Domain.Entities;
using Mapster;

namespace Academy.Application.Sport.Command.Handlers
{
    public class DeleteSportRequestHandler : IRequestHandler<DeleteSportRequest, Result<SportsDto>>
    {
        private readonly IRepository<Sports> _repository;
        public DeleteSportRequestHandler(IRepository<Sports> repository)
        {
            _repository = repository;
        }
        public async Task<Result<SportsDto>> Handle(DeleteSportRequest request, CancellationToken cancellationToken)
        {
            var SportToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (SportToDelete == null)
                return Result.Fail(new NotFoundException("Common Lookup Not Found"));

            await _repository.DeleteAsync(SportToDelete, cancellationToken);
            return Result.Succeed(SportToDelete.Adapt<SportsDto>());
        }
    }
}
