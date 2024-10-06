using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Infrastructure.Persistence.Configuration
{
    public class UserInfoConfig : AuditableEntityConfig<UserInfo>, IEntityTypeConfiguration<UserInfo>
    {
        public override void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            base.Configure(builder);
        }
    }
}
