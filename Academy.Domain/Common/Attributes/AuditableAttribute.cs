namespace Academy.Domain.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class AuditableAttribute : Attribute
    {
        public bool IsAudited { get; }

        public AuditableAttribute(bool isAudited = true)
        {
            IsAudited = isAudited;
        }
    }
}
