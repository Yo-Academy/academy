using Academy.Application.Common.Exceptions;
using Academy.Application.Persistence.Repository;
using Academy.Application.Sport.Command.Models;
using Academy.Application.Sport.Dto;
using Academy.Domain.Entities;
using Mapster;

namespace Academy.Application.Sport.Command.Handlers
{
    public class UpdateSportRequestHandler : IRequestHandler<UpdateSportRequest, Result<SportsDto>>
    {
        private readonly IRepository<Sports> _repository;
        public UpdateSportRequestHandler(IRepository<Sports> repository)
        {
            _repository = repository;
        }
        public async Task<Result<SportsDto>> Handle(UpdateSportRequest request, CancellationToken cancellationToken)
        {
            var SportToUpdate = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (SportToUpdate == null)
                return Result.Fail(new NotFoundException("Sport Not Found"));

            SportToUpdate.Update(request.Name, request.Image, request.IsActive);

            await _repository.UpdateAsync(SportToUpdate, cancellationToken);
            return Result.Succeed(SportToUpdate.Adapt<SportsDto>());
        }
    }
}
