namespace Academy.Domain.Common.Contracts
{
    public interface IAuditableEntity
    {
        public DefaultIdType CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DefaultIdType LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}