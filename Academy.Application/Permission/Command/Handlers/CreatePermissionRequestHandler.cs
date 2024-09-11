using Academy.Application.Contracts;
using Academy.Application.Permission.Command.Models;
using Academy.Application.Permission.Dto;
using Academy.Domain.Entities;
using Mapster;
using Academy.Application.Contracts.Persistence;

namespace Academy.Application.Permission.Command.Handlers
{
    public class CreatePermissionRequestHandler : IRequestHandler<CreatePermissionRequest, Result<PermissionDto>>
    {
        private readonly IRepository<Permissions> _repository;

        public CreatePermissionRequestHandler(IRepository<Permissions> repository)
        {
            _repository = repository;
        }
        public async Task<Result<PermissionDto>> Handle(CreatePermissionRequest request, CancellationToken cancellationToken)
        {
            var id = DefaultIdType.NewGuid();
            Permissions Permission = new Permissions(id, request.Action, request.Resource, request.Description, request.IsBasic, request.IsRoot);

            var responsePermission = await _repository.AddAsync(Permission);

            return Result.Succeed(responsePermission.Adapt<PermissionDto>());
        }
    }

}
