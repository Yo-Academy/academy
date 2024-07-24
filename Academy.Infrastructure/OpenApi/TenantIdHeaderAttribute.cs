using Academy.Shared.Multitenancy;

namespace Academy.Infrastructure.OpenApi
{
    public class TenantIdHeaderAttribute : SwaggerHeaderAttribute
    {
        public TenantIdHeaderAttribute()
            : base(
                MultitenancyConstants.TenantIdName,
                "Input your tenant Id to access this API",
                "root",
                false)
        {
        }
    }
}
