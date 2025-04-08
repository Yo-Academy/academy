using Academy.Application.UserPaymentInfo.Dto;

namespace Academy.Application.UserPaymentInfo.Command.Models
{
    public class UpdateUserPaymentInfoRequest : IRequest<Result<UserPaymentInfoDto>>
    {
        public DefaultIdType Id { get; set; }
        public DefaultIdType PaymentTypeId { get; set; }
        public DefaultIdType UserInfoId { get; set; }
        public string ReceiptNumber { get; set; }
        public DateOnly ReceiptDate { get; set; }
        public DateOnly PaymentDate { get; set; }
        public DateOnly SubscriptionStartDate { get; set; }
        public DateOnly SubscriptionEndDate { get; set; }
        public double Fee { get; set; }
        public string Remark { get; set; }
        public string PaymentImage { get; set; }
    }
}
