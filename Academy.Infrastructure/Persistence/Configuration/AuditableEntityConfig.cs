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

        //// Define foreign key relationships with ApplicationUser
        //builder.HasOne<ApplicationUser>()
        //    .WithMany() // Assuming no navigation property in ApplicationUser
        //    .HasForeignKey(p => p.CreatedBy)
        //    .HasConstraintName("FK_Entity_ApplicationUser_CreatedBy")
        //    .OnDelete(DeleteBehavior.SetNull); // Set to null on user deletion

        //builder.HasOne<ApplicationUser>()
        //.WithMany() // Assuming no navigation property in ApplicationUser
        //.HasForeignKey(p => p.LastModifiedBy)
        //.HasConstraintName("FK_Entity_ApplicationUser_LastModifiedBy")
        //.OnDelete(DeleteBehavior.SetNull); // Set to null on user deletion
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
