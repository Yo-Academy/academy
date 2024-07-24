using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Infrastructure.Persistence.Configuration
{
    public class CommonLookupConfig : IEntityTypeConfiguration<CommonLookup>
    {
        public void Configure(EntityTypeBuilder<CommonLookup> builder)
        {
            builder.IsMultiTenant();
        }
    }
}
