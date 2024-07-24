using Academy.Application.Common.Events;
using Academy.Application.Common.Interfaces;
using Academy.Domain.Common.Attributes;
using Academy.Domain.Common.Contracts;
using Academy.Infrastructure.Auditing;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System.Data;

namespace Academy.Infrastructure.Persistence.Context
{
    public abstract class BaseDbContext : MultiTenantIdentityDbContext<ApplicationUser, ApplicationRole, DefaultIdType, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        protected readonly ICurrentUser _currentUser;
        private readonly ISerializerService _serializer;
        private readonly DatabaseSettings _dbSettings;
        private readonly IEventPublisher _events;
        private readonly AuditingSettings _auditingSettings;

        protected BaseDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events,
            IOptions<AuditingSettings> auditingSettings)
            : base(currentTenant, options)
        {
            _currentUser = currentUser;
            _serializer = serializer;
            _dbSettings = dbSettings.Value;
            _events = events;
            _auditingSettings = auditingSettings.Value;
        }

        // Used by Dapper
        public IDbConnection Connection => Database.GetDbConnection();

        public DbSet<Trail> AuditTrails => Set<Trail>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // QueryFilters need to be applied before base.OnModelCreating
            modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedOn == null);

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: We want this only for development probably... maybe better make it configurable in logger.json config?
            optionsBuilder.EnableSensitiveDataLogging();

            // If you want to see the sql queries that efcore executes:

            // Uncomment the next line to see them in the output window of visual studio
            // optionsBuilder.LogTo(m => System.Diagnostics.Debug.WriteLine(m), Microsoft.Extensions.Logging.LogLevel.Information);

            // Or uncomment the next line if you want to see them in the console
            // optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);

            if (!string.IsNullOrWhiteSpace(TenantInfo?.ConnectionString))
            {
                optionsBuilder.UseDatabase(_dbSettings.DBProvider, TenantInfo.ConnectionString);
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateAuditableField(_currentUser.GetUserId());

            List<AuditTrail> auditEntries = [];
            if (_auditingSettings.Enable)
                auditEntries = HandleAuditingBeforeSaveChanges(_currentUser.GetUserId());

            int result = await base.SaveChangesAsync(cancellationToken);

            if (_auditingSettings.Enable)
                await HandleAuditingAfterSaveChangesAsync(auditEntries, cancellationToken);

            await SendDomainEventsAsync();

            return result;
        }

        private List<AuditTrail> HandleAuditingBeforeSaveChanges(DefaultIdType userId)
        {
            ChangeTracker.DetectChanges();

            var trailEntries = new List<AuditTrail>();
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>()
                .Where(x => x.Entity.GetType().GetCustomAttributes(typeof(AuditableAttribute), true).FirstOrDefault() is AuditableAttribute auditableAttribute && auditableAttribute.IsAudited)
                .Where(e => e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified)
                .ToList())
            {
                var entityType = entry.Entity.GetType();

                var trailEntry = new AuditTrail(entry, _serializer)
                {
                    TableName = entityType.Name,
                    UserId = userId
                };

                foreach (var property in entry.Properties
                    .Where(x => x.Metadata.PropertyInfo?.GetCustomAttributes(typeof(AuditableAttribute), true).FirstOrDefault() is not AuditableAttribute auditableAttribute || auditableAttribute.IsAudited))
                {
                    if (property.IsTemporary)
                    {
                        trailEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        trailEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            trailEntry.TrailType = TrailType.Create;
                            trailEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            trailEntry.TrailType = TrailType.Delete;
                            trailEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified && entry.Entity is ISoftDelete && property.OriginalValue == null && property.CurrentValue != null)
                            {
                                trailEntry.ChangedColumns.Add(propertyName);
                                trailEntry.TrailType = TrailType.Delete;
                                trailEntry.OldValues[propertyName] = property.OriginalValue;
                                trailEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            else if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                            {
                                trailEntry.ChangedColumns.Add(propertyName);
                                trailEntry.TrailType = TrailType.Update;
                                trailEntry.OldValues[propertyName] = property.OriginalValue;
                                trailEntry.NewValues[propertyName] = property.CurrentValue;
                            }

                            break;
                    }
                }

                if (trailEntry.TrailType != TrailType.None) trailEntries.Add(trailEntry);
            }

            foreach (var auditEntry in trailEntries.Where(e => !e.HasTemporaryProperties))
            {
                AuditTrails.Add(auditEntry.ToAuditTrail());
            }

            return trailEntries.Where(e => e.HasTemporaryProperties).ToList();
        }

        private void UpdateAuditableField(DefaultIdType userId)
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = userId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = userId;
                        break;

                    case EntityState.Deleted:
                        if (entry.Entity is ISoftDelete softDelete)
                        {
                            softDelete.DeletedBy = userId;
                            softDelete.DeletedOn = DateTime.UtcNow;
                            softDelete.IsDeleted = true;
                            entry.State = EntityState.Modified;
                        }

                        break;
                }
            }
        }

        private Task HandleAuditingAfterSaveChangesAsync(List<AuditTrail> trailEntries, CancellationToken cancellationToken = new())
        {
            if (trailEntries == null || trailEntries.Count == 0)
            {
                return Task.CompletedTask;
            }

            foreach (var entry in trailEntries)
            {
                foreach (var prop in entry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        entry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        entry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                AuditTrails.Add(entry.ToAuditTrail());
            }

            return SaveChangesAsync(cancellationToken);
        }

        private async Task SendDomainEventsAsync()
        {
            var entitiesWithEvents = ChangeTracker.Entries<IEntity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Count > 0)
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var domainEvents = entity.DomainEvents.ToArray();
                entity.DomainEvents.Clear();
                foreach (var domainEvent in domainEvents)
                {
                    await _events.PublishAsync(domainEvent);
                }
            }
        }
    }
}