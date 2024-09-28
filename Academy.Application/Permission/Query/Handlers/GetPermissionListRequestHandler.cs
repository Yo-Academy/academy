using Academy.Application.Academies.Dto;
using Academy.Application.Permission.Dto;
using Academy.Application.Permission.Query.Models;
using Academy.Application.Permission.Specifications;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;
using Mapster;

namespace Academy.Application.Permission.Query.Handlers
{
    public class GetPermissionListRequestHandler : IRequestHandler<GetPermissionListRequest, Result<PaginationResponse<PermissionDto>>>
    {
        private readonly IReadRepository<Permissions> _repository;
        public GetPermissionListRequestHandler(IReadRepository<Permissions> repository)
        {
            _repository = repository;
        }
        public async Task<Result<PaginationResponse<PermissionDto>>> Handle(GetPermissionListRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetPermissionListSpec(request);
            var data = await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);

            if (data != null && data.Data.Count > 0)
            {
                var PermissionList = (data.Data.Adapt<List<PermissionDto>>());
                data.Data = PermissionList;
            }
            return Result.Succeed(data);
        }
    }
}
