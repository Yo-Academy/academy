using Academy.Application.Persistence.Repository;
using Academy.Application.UserInfo.Dto;
using Academy.Application.UserInfo.Query.Models;
using Mapster;
using Entity = Academy.Domain.Entities;

namespace Academy.Application.UserInfo.Query.Handlers
{
    public class GetUserInfoDetailHandler : IRequestHandler<GetUserInfoDetailRequest, Result<UserInfoDto>>
    {
        private readonly IReadRepository<Entity.UserInfo> _userInfoReadRepository;

        public GetUserInfoDetailHandler(IReadRepository<Entity.UserInfo> userInfoReadRepository)
        {
            _userInfoReadRepository = userInfoReadRepository;
        }
        public async Task<Result<UserInfoDto>> Handle(GetUserInfoDetailRequest request, CancellationToken cancellationToken)
        {
            var userInfoDetail = new UserInfoDto();
            var data = await _userInfoReadRepository.GetByIdAsync(request.Id, cancellationToken);
            if (data != null)
            {
                userInfoDetail = data.Adapt<UserInfoDto>();
            }
            return Result.Succeed(userInfoDetail);
        }
    }
}
