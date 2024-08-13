using Academy.Domain.Common.Contracts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Infrastructure.Persistence.Configuration;
public class AuditableEntityConfig<T> : IEntityTypeConfiguration<T> where T : AuditableEntity
{
	public virtual void Configure(EntityTypeBuilder<T> builder)
	{
		builder.Property(p => p.Id)
				.HasColumnName("id")
				.IsRequired();

		builder.Property(p => p.CreatedBy)
			.HasColumnName("created_by")
            .HasMaxLength(100)
			.IsRequired();

		builder.Property(p => p.CreatedOn)
			.HasColumnName("created_on")
			.IsRequired();

        builder.Property(p => p.LastModifiedBy)
            .HasColumnName("last_modified_by")
            .HasMaxLength(100)
            .IsRequired();

		builder.Property(p => p.LastModifiedOn)
			.HasColumnName("last_modified_on");

		builder.Property(p => p.DeletedBy)
            .HasMaxLength(100)
			.HasColumnName("deleted_by");

		builder.Property(p => p.DeletedOn)
			.HasColumnName("deleted_on");

		builder.Property(p => p.IsDeleted)
			.HasColumnName("is_deleted")
			.IsRequired();
	}
}

//public class AuditableEntityStringConfig<T> : IEntityTypeConfiguration<T> where T : AuditableEntity<string>
//{
//    public virtual void Configure(EntityTypeBuilder<T> builder)
//    {
//        builder.Property(p => p.Id)
//                .HasColumnName("id")
//                .HasMaxLength(100)
//                .IsRequired();

//        builder.Property(p => p.CreatedBy)
//            .HasColumnName("created_by")
//            .HasMaxLength(100)
//            .IsRequired();

//        builder.Property(p => p.CreatedOn)
//            .HasColumnName("created_on")
//            .IsRequired();

//        builder.Property(p => p.LastModifiedBy)
//            .HasColumnName("last_modified_by")
//            .HasMaxLength(100)
//            .IsRequired();

//        builder.Property(p => p.LastModifiedOn)
//            .HasColumnName("last_modified_on");

//        builder.Property(p => p.DeletedBy)
//            .HasMaxLength(100)
//            .HasColumnName("deleted_by");

//        builder.Property(p => p.DeletedOn)
//            .HasColumnName("deleted_on");

//        builder.Property(p => p.IsDeleted)
//            .HasColumnName("is_deleted")
//            .IsRequired();
//    }
//}
