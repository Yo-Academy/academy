using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Infrastructure.Persistence.Configuration
{
    public class CoachingConfig : AuditableEntityConfig<Coaching>, IEntityTypeConfiguration<Coaching>
    {
        public override void Configure(EntityTypeBuilder<Coaching> builder)
        {
            base.Configure(builder);
        }
    }
}
