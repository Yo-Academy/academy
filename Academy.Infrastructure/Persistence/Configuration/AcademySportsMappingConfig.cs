using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Infrastructure.Persistence.Configuration
{
    public class AcademySportsMappingConfig : AuditableEntityConfig<AcademySportsMapping>, IEntityTypeConfiguration<AcademySportsMapping>
    {
        public override void Configure(EntityTypeBuilder<AcademySportsMapping> builder)
        {
            base.Configure(builder);

            builder.IsMultiTenant();
        }
    }
}
