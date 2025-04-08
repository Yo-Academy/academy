namespace Academy.Domain.Entities
{
    public class UserPaymentInfo :  AuditableEntity, IAggregateRoot
    {
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

        [ForeignKey("PaymentTypeId")]
        public virtual PaymentType PaymentType { get; set; }
        [ForeignKey("UserInfoId")]
        public virtual UserInfo UserInfo { get; set; }

        public UserPaymentInfo()
        {

        }

        public UserPaymentInfo(DefaultIdType id, DefaultIdType paymentTypeId, DefaultIdType userInfoId, string receiptNumber, DateOnly receiptDate, DateOnly paymentDate, DateOnly subscriptionStartDate, DateOnly subscriptionEndDate, double fee, string remark, string paymentImage)
        {
            Id = id;
            PaymentTypeId = paymentTypeId;
            UserInfoId = userInfoId;
            ReceiptNumber = receiptNumber;
            ReceiptDate = receiptDate;
            PaymentDate = paymentDate;
            SubscriptionStartDate = subscriptionStartDate;
            SubscriptionEndDate = subscriptionEndDate;
            Fee = fee;
            Remark = remark;
            PaymentImage = paymentImage;
        }

        public UserPaymentInfo Update(DefaultIdType paymentTypeId, DefaultIdType userInfoId, string receiptNumber, DateOnly receiptDate, DateOnly paymentDate, DateOnly subscriptionStartDate, DateOnly subscriptionEndDate, double fee, string remark, string paymentImage)
        {
            PaymentTypeId = paymentTypeId;
            UserInfoId = userInfoId;
            ReceiptNumber = receiptNumber;
            ReceiptDate = receiptDate;
            PaymentDate = paymentDate;
            SubscriptionStartDate = subscriptionStartDate;
            SubscriptionEndDate = subscriptionEndDate;
            Fee = fee;
            Remark = remark;
            PaymentImage = paymentImage;
            return this;
        }
    }
}
