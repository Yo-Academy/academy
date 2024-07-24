using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Infrastructure.Persistence.Configuration
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .ToTable("Users")
                .IsMultiTenant();

            builder
                .Property(u => u.ObjectId)
                    .HasMaxLength(256);
        }
    }

    public class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder) =>
            builder
                .ToTable("Roles")
                .IsMultiTenant()
                    .AdjustUniqueIndexes();
    }

    public class ApplicationRoleClaimConfig : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder) =>
            builder
                .ToTable("RoleClaims")
                .IsMultiTenant();
    }

    public class ApplicationUserRoleConfig : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder) =>
            builder
                .ToTable("UserRoles")
                .IsMultiTenant();
    }

    public class ApplicationUserClaimConfig : IEntityTypeConfiguration<ApplicationUserClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder) =>
            builder
                .ToTable("UserClaims")
                .IsMultiTenant();
    }

    public class ApplicationUserLoginConfig : IEntityTypeConfiguration<ApplicationUserLogin>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder) =>
            builder
                .ToTable("UserLogins")
                .IsMultiTenant();
    }

    public class ApplicationUserTokenConfig : IEntityTypeConfiguration<ApplicationUserToken>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserToken> builder) =>
            builder
                .ToTable("UserTokens")
                .IsMultiTenant();
    }
}