namespace Academy.Domain.Entities
{
    public class Permissions : AuditableEntity, IAggregateRoot
    {
        public string Action { get; set; } = default!;
        public string Resource { get; set; } = default!;
        public string? Description { get; set; }
        public string? Name { get; set; }
        public bool IsBasic { get; set; }
        public bool IsRoot { get; set; }
        public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";

        public Permissions()
        {

        }

        public Permissions(Guid id, string action, string resource, string? description, bool isBasic, bool isRoot)
        {
            Id = id;
            Action = action;
            Resource = resource;
            Description = description;
            IsBasic = isBasic;
            IsRoot = isRoot;
            Name = NameFor(action, resource);
        }

        public void Update(string action, string resource, string? description, bool isBasic, bool isRoot)
        {
            Action = action;
            Resource = resource;
            Description = description;
            IsBasic = isBasic;
            IsRoot = isRoot;
        }
    }
}
