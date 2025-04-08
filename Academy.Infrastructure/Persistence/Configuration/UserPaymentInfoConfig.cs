using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Infrastructure.Persistence.Configuration
{
    public class UserPaymentInfoConfig : AuditableEntityConfig<UserPaymentInfo>, IEntityTypeConfiguration<UserPaymentInfo>
    {
        public override void Configure(EntityTypeBuilder<UserPaymentInfo> builder)
        {
            base.Configure(builder);
        }
    }
}
