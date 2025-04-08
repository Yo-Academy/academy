using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Infrastructure.Persistence.Configuration
{
    public class PaymentTypeConfig : AuditableEntityConfig<PaymentType>, IEntityTypeConfiguration<PaymentType>
    {
        public override void Configure(EntityTypeBuilder<PaymentType> builder)
        {
            base.Configure(builder);
        }
    }
}
