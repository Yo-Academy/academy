using Academy.Application.Common.Exceptions;
using Academy.Application.Permission.Command.Models;
using Academy.Application.Permission.Dto;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Mapster;

namespace Academy.Application.Permission.Command.Handlers
{
    public class UpdatePermissionRequestHandler : IRequestHandler<UpdatePermissionRequest, Result<PermissionDto>>
    {
        private readonly IRepository<Permissions> _repository;
        public UpdatePermissionRequestHandler(IRepository<Permissions> repository)
        {
            _repository = repository;
        }
        public async Task<Result<PermissionDto>> Handle(UpdatePermissionRequest request, CancellationToken cancellationToken)
        {
            var PermissionToUpdate = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (PermissionToUpdate == null)
                return Result.Fail(new NotFoundException(DbRes.T("PermissionNotFound")));

            PermissionToUpdate.Update(request.Action, request.Resource, request.Description, request.IsBasic, request.IsRoot);

            await _repository.UpdateAsync(PermissionToUpdate, cancellationToken);
            return Result.Succeed(PermissionToUpdate.Adapt<PermissionDto>());
        }
    }
}
