using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Infrastructure.Persistence.Configuration
{
    public class SportsConfig : AuditableEntityConfig<Sports>, IEntityTypeConfiguration<Sports>
    {
        public override void Configure(EntityTypeBuilder<Sports> builder)
        {
            base.Configure(builder);

            //builder.IsMultiTenant();
        }
    }
}
