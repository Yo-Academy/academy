namespace Academy.Domain.Identity
{
    [Auditable]
    public class ApplicationUserRole : IdentityUserRole<DefaultIdType>, IAuditableEntity
    {
        public DefaultIdType CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DefaultIdType LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
