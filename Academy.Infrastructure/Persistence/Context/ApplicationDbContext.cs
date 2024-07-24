using Academy.Application.Common.Events;
using Academy.Infrastructure.Auditing;
using Finbuckle.MultiTenant;
using Microsoft.Extensions.Options;
using Entity = Academy.Domain.Entities;


namespace Academy.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : BaseDbContext
    {
        public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events,
            IOptions<AuditingSettings> auditingSettings)
            : base(currentTenant, options, currentUser, serializer, dbSettings, events, auditingSettings)
        {
        }

        public DbSet<Setting> Settings => Set<Setting>();

        public DbSet<Entity.EmailHistory> EmailHistory => Set<Entity.EmailHistory>();

        public DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        public DbSet<CommonLookup> CommonLookups => Set<CommonLookup>();

        public DbSet<CommonLookupTranslation> CommonLookupTranslations => Set<CommonLookupTranslation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>().ToTable(nameof(AuditLogs), t => t.ExcludeFromMigrations());
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CommonLookupTranslation>()
               .HasOne(l => l.CommonLookup)
               .WithMany(l => l.CommonLookupTranslations)
               .HasForeignKey(l => l.CommonLookupId);
        }
    }
}