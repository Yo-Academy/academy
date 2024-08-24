namespace Academy.Domain.Entities
{
    public class Coaching : AuditableEntity, IAggregateRoot
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public Coaching()
        {
        }

        public Coaching(DefaultIdType id, string code, string name)
        {
            this.Id =id;
            this.Code =code;
            this.Name = name;
            
        }
    }
}
