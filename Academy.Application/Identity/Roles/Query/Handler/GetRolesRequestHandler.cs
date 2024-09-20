using Academy.Application.Academies.Dto;
using Academy.Application.Academies.Query.Models;
using Academy.Application.Academies.Specifications;
using Academy.Application.Identity.Roles.Dto;
using Academy.Application.Identity.Roles.Query.Models;
using Academy.Application.Identity.Roles.Specifications;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Identity;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;
using Mapster;
using Entity = Academy.Domain.Entities;

namespace Academy.Application.Identity.Roles.Query.Handlers
{
    public class GetRolesRequestHandler : IRequestHandler<GetRolesListRequest, Result<PaginationResponse<RoleDto>>>
    {
        private readonly IReadOnlyRepository<ApplicationRole> _repository;
        public GetRolesRequestHandler(IReadOnlyRepository<ApplicationRole> repository)
        {
            _repository = repository;
        }

        public async Task<Result<PaginationResponse<RoleDto>>> Handle(GetRolesListRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetRolesListSpec(request);
            var data = await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);


            if (data.Data != null && data.Data.Count > 0)
            {
                var roleList = data.Data.Adapt<List<RoleDto>>();
                data.Data = roleList;
            }
            return Result.Succeed(data);
        }
    }
}
