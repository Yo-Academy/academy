namespace Academy.Domain.Entities
{
    public class Setting : AuditableEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;
        public string? Type { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
