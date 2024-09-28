using Academy.Application.Identity.Users.Query.Models;
using Academy.Application.Identity.Users.Specifications;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Identity;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;
using Mapster;

namespace Academy.Application.Identity.Users.Query.Handlers
{
    public class GetUsersListRequestHandler : IRequestHandler<GetUsersListRequest, Result<PaginationResponse<UserDetailsDto>>>
    {
        private readonly IReadOnlyRepository<ApplicationUser> _repository;
        public GetUsersListRequestHandler(IReadOnlyRepository<ApplicationUser> repository)
        {
            _repository = repository;
        }
        public async Task<Result<PaginationResponse<UserDetailsDto>>> Handle(GetUsersListRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetUsersListSpec(request);
            var data = await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);

            if (data != null && data.Data.Count > 0)
            {
                var UsersList = data.Data.Adapt<List<UserDetailsDto>>();
                data.Data = UsersList;
            }
            return Result.Succeed(data);
        }
    }
}
