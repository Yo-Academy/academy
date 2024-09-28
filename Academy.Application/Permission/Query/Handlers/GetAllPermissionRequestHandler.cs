using Academy.Application.Contracts;
using Academy.Application.Permission.Dto;
using Academy.Application.Permission.Query.Models;
using Academy.Application.Permission.Specifications;
using Entity = Academy.Domain.Entities;
using Mapster;
using Academy.Shared.Authorization;
using Academy.Application.Persistence.Repository;

namespace Academy.Application.Permission.Query.Handlers
{
    public class GetAllPermissionRequestHandler : IRequestHandler<GetAllPermissionRequest, Result<List<PermissionListDto>>>
    {
        private readonly IReadRepository<Entity.Permissions> _repository;
        public GetAllPermissionRequestHandler(IReadRepository<Entity.Permissions> repository)
        {
            _repository = repository;
        }
        public async Task<Result<List<PermissionListDto>>> Handle(GetAllPermissionRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetAllPermissionSpec(request);
            var data = await _repository.ListAsync(cancellationToken);

            if (data != null && data.Count > 0)
            {
                var filteredData = data.Where(x => x.Resource != Resource.Permission).ToList();
                var PermissionList = (filteredData.Adapt<List<PermissionListDto>>());
                return Result.Succeed(PermissionList);
            }
            return Result.Succeed(new List<PermissionListDto>());
        }
    }
}
