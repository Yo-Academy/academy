using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Infrastructure.Persistence.Configuration
{
    public class BatchConfig : AuditableEntityConfig<Batch>, IEntityTypeConfiguration<Batch>
    {
        public override void Configure(EntityTypeBuilder<Batch> builder)
        {
            base.Configure(builder);
        }
    }
}
