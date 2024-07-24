using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Academy.Domain.Entities;

namespace Academy.Infrastructure.Persistence.Configuration
{
    internal class EmailHistoryConfig : IEntityTypeConfiguration<Entities.EmailHistory>
    {
        public void Configure(EntityTypeBuilder<Entities.EmailHistory> builder)
        {
            builder.IsMultiTenant();
        }
    }
}
