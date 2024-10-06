using Academy.Application.UserInfo.Dto;

namespace Academy.Application.UserInfo.Command.Models
{
    public class DeleteUserInfoRequest : IRequest<Result<UserInfoDto>>
    {
        public DefaultIdType Id { get; set; }
    }
}
