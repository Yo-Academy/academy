using Finbuckle.MultiTenant.Stores;

namespace Academy.Infrastructure.Multitenancy
{
    public class TenantDbContext : EFCoreStoreDbContext<TenantInfo>
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TenantInfo>().ToTable("Tenants");
        }
    }
}
