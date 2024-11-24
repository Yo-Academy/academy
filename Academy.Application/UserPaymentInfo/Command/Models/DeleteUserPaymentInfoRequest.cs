using Academy.Application.UserPaymentInfo.Dto;

namespace Academy.Application.UserPaymentInfo.Command.Models
{
    public class DeleteUserPaymentInfoRequest : IRequest<Result<UserPaymentInfoDto>>
    {
        public DefaultIdType Id { get; set; }
    }
}
