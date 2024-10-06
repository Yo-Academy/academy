using Academy.Application.UserInfo.Dto;

namespace Academy.Application.UserInfo.Query.Models
{
    public class GetUserInfoDetailRequest : IRequest<Result<UserInfoDto>>
    {
        public DefaultIdType Id { get; set; }
    }
   
}
