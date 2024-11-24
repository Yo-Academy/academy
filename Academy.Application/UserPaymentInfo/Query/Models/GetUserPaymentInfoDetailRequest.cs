using Academy.Application.UserPaymentInfo.Dto;

namespace Academy.Application.UserPaymentInfo.Query.Models
{
    public class GetUserPaymentInfoDetailRequest : IRequest<Result<UserPaymentInfoDto>>
    {
        public DefaultIdType Id { get; set; }
    }
}
