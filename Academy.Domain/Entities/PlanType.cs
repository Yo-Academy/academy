namespace Academy.Domain.Entities
{
    public class PlanType : AuditableEntity, IAggregateRoot
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public PlanType()
        {
        }

        public PlanType(DefaultIdType id, string code, string name)
        {
            this.Id = id;
            this.Code = code;
            this.Name = name;

        }
    }
}
