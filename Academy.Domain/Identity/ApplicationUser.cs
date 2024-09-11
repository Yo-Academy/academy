namespace Academy.Domain.Identity
{
    [Auditable]
    public class ApplicationUser : IdentityUser<DefaultIdType>, IAuditableEntity, ISoftDelete
    {
        public ApplicationUser()
        {
            Id = NewId.Next().ToGuid();
            SecurityStamp = NewId.Next().ToGuid().ToString();
        }

        public ApplicationUser(string userName) : this()
        {
            UserName = userName;
        }
        public override string? Email { get => base.Email ; set => base.Email = value; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        [Auditable(false)]
        public string? RefreshToken { get; set; }
        [Auditable(false)]
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? ObjectId { get; set; }
        [Auditable(false)]
        public override string? ConcurrencyStamp { get; set; } = NewId.Next().ToGuid().ToString();
        public DefaultIdType CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DefaultIdType LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DefaultIdType? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string? OTP { get; set; }
        public string CountryCode { get; set; }
        public string TenantId { get; set; }
        public virtual ICollection<ApplicationUserRole> ApplicationUserRole { get; set; }
    }
}