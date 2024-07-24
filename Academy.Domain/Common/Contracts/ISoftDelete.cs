namespace Academy.Domain.Common.Contracts
{
    public interface ISoftDelete
    {
        DateTime? DeletedOn { get; set; }
        DefaultIdType? DeletedBy { get; set; }
        bool IsDeleted { get; set; }
    }
}