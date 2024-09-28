namespace Academy.Domain.Identity
{
    [Auditable]
    public class ApplicationRole : IdentityRole<DefaultIdType>, IAuditableEntity, ISoftDelete
    {
        public ApplicationRole()
        {
            Id = NewId.Next().ToGuid();
        }

        public ApplicationRole(string roleName) : this()
        {
            Name = roleName;
        }

        public string? Description { get; set; }
        public override string? ConcurrencyStamp { get; set; } = NewId.Next().ToGuid().ToString();
        public DefaultIdType CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DefaultIdType LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DefaultIdType? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        public ApplicationRole(string name, string? description = null)
            : base(name)
        {
            Description = description;
            NormalizedName = name.ToUpperInvariant();
        }
    }
}