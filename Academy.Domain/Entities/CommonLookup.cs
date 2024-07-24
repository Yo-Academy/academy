namespace Academy.Domain.Entities
{
    public class CommonLookup : AuditableEntity, IAggregateRoot
    {
        public string Category { get; set; } = default!;
        public string Key { get; set; } = default!;
        public string? Description { get; set; }
        public int? DisplayOrder { get; set; }
        public ICollection<CommonLookupTranslation> CommonLookupTranslations { get; set; }

        public CommonLookup() { }

        public CommonLookup(DefaultIdType id, string category, string key, string description, int? display_order)
        {
            Id = id;
            Category = category;
            Key = key;
            Description = description;
            DisplayOrder = display_order;
        }

        public CommonLookup Update(string category, string key, string description, int? display_order)
        {
            Category = (category != null && this.Category.Equals(category) is not true) ? category : this.Category;
            Key = (key != null && !this.Key.Equals(category)) ? key : this.Key;
            Description = (Description != this.Description) ? description : this.Description;
            DisplayOrder = (display_order != this.DisplayOrder) ? display_order : this.DisplayOrder;
            return this;
        }
    }
}
