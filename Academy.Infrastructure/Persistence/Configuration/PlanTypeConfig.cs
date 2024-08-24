using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Infrastructure.Persistence.Configuration
{
    public class PlanTypeConfig : AuditableEntityConfig<PlanType>, IEntityTypeConfiguration<PlanType>
    {
        public override void Configure(EntityTypeBuilder<PlanType> builder)
        {
            base.Configure(builder);
        }
    }
}
