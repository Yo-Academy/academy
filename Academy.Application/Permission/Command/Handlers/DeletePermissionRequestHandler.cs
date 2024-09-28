using Academy.Domain.Entities;
using Academy.Application.Permission.Command.Models;
using Academy.Application.Permission.Dto;
using Academy.Application.Common.Exceptions;
using Mapster;
using Academy.Application.Persistence.Repository;

namespace Academy.Application.Permission.Command.Handlers
{
    public class DeletePermissionRequestHandler : IRequestHandler<DeletePermissionRequest, Result<PermissionDto>>
    {
        private readonly IRepository<Permissions> _repository;
        public DeletePermissionRequestHandler(IRepository<Permissions> repository)
        {
            _repository = repository;
        }
        public async Task<Result<PermissionDto>> Handle(DeletePermissionRequest request, CancellationToken cancellationToken)
        {
            var PermissionToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (PermissionToDelete == null)
                return Result.Fail(new NotFoundException(DbRes.T("PermissionNotFound")));

            await _repository.DeleteAsync(PermissionToDelete, cancellationToken);
            return Result.Succeed(PermissionToDelete.Adapt<PermissionDto>());
        }
    }
}
