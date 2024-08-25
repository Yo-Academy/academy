using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Infrastructure.Persistence.Configuration
{
    public class SubscriptionConfig : AuditableEntityConfig<Subscription>, IEntityTypeConfiguration<Subscription>
    {
        public override void Configure(EntityTypeBuilder<Subscription> builder)
        {
            base.Configure(builder);
        }
    }
}
