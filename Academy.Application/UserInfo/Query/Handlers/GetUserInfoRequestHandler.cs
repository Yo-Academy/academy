using Academy.Application.Persistence.Repository;
using Academy.Application.UserInfo.Dto;
using Academy.Application.UserInfo.Query.Models;
using Academy.Application.UserInfo.Specifications;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;
using Mapster;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.UserInfo.Query.Handlers
{
    public class GetUserInfoListRequestHandler : IRequestHandler<GetUserInfoListRequest, Result<PaginationResponse<UserInfoDto>>>
    {
        private readonly IReadRepository<Entites.UserInfo> _repository;
        public GetUserInfoListRequestHandler(IReadRepository<Entites.UserInfo> repository)
        {
            _repository = repository;
        }

        public async Task<Result<PaginationResponse<UserInfoDto>>> Handle(GetUserInfoListRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetUserInfoListSpec(request);
            var data = await _repository.PaginatedListAsync(spec,
                                                           request.PageNumber,
                                                           request.PageSize,
                                                           cancellationToken);

            if (data != null)
            {
                var userInfoList = data.Data.Adapt<List<UserInfoDto>>();
                data.Data = userInfoList;

            }
            return Result.Succeed(data);
        }
    }
}
