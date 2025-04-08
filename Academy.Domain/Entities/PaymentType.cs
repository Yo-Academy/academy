namespace Academy.Domain.Entities
{
    public class PaymentType : AuditableEntity, IAggregateRoot
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public PaymentType()
        {
        }

        public PaymentType(DefaultIdType id, string code, string name)
        {
            this.Id = id;
            this.Code = code;
            this.Name = name;

        }
    }
}
