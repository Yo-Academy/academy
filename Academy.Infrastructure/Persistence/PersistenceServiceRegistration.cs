using Academy.Application.Contracts.Persistence;
using Academy.Infrastructure.Auditing;
using Academy.Infrastructure.Common;
using Academy.Infrastructure.Persistence.ConnectionString;
using Academy.Infrastructure.Persistence.Context;
using Academy.Infrastructure.Persistence.Initialization;
using Academy.Infrastructure.Persistence.Repositories;
using Academy.Infrastructure.Persistence.Repository;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Serilog;
using REPO = Academy.Application.Persistence.Repository;

namespace Academy.Infrastructure.Persistence
{
    internal static class PersistenceServiceRegistration
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(PersistenceServiceRegistration));

        internal static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddOptions<DatabaseSettings>()
                .BindConfiguration(nameof(DatabaseSettings))
                .PostConfigure(databaseSettings =>
                {
                    _logger.Information("Current DB Provider: {dbProvider}", databaseSettings.DBProvider);
                })
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<AuditingSettings>()
                .BindConfiguration(nameof(AuditingSettings));

            return services
                .AddDbContext<ApplicationDbContext>((p, options) =>
                {
                    var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                    options.UseDatabase(databaseSettings.DBProvider, databaseSettings.ConnectionString);
                })

                .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
                .AddTransient<ApplicationDbInitializer>()
                .AddTransient<ApplicationDbSeeder>()
                .AddServices(typeof(ICustomSeeder), ServiceLifetime.Transient)
                .AddTransient<CustomSeederRunner>()

                .AddTransient<IConnectionStringSecurer, ConnectionStringSecurer>()
                .AddTransient<IConnectionStringValidator, ConnectionStringValidator>()

                .AddRepositories();
        }

        internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
        {
            return dbProvider.ToLowerInvariant() switch
            {
                DbProviderKeys.Npgsql => builder.UseNpgsql(connectionString, e =>
                                     e.MigrationsAssembly("Academy.Migrators.PostgreSQL")),
                _ => throw new InvalidOperationException($"DB Provider {dbProvider} is not supported."),
            };
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(REPO.IRepository<>), typeof(DbRepository<>));
            services.AddScoped(typeof(REPO.IReadRepository<>), typeof(DbRepository<>));


            //    // Add Repositories
            //    services.AddScoped(typeof(IRepository<>), typeof(ApplicationDbRepository<>));

            //    foreach (var aggregateRootType in
            //        typeof(IAggregateRoot).Assembly.GetExportedTypes()
            //            .Where(t => typeof(IAggregateRoot).IsAssignableFrom(t) && t.IsClass)
            //            .ToList())
            //    {
            //        // Add ReadRepositories.
            //        services.AddScoped(typeof(IReadRepository<>).MakeGenericType(aggregateRootType), sp =>
            //            sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType)));

            //        // Decorate the repositories with EventAddingRepositoryDecorators and expose them as IRepositoryWithEvents.
            //        services.AddScoped(typeof(IRepositoryWithEvents<>).MakeGenericType(aggregateRootType), sp =>
            //            Activator.CreateInstance(
            //                typeof(EventAddingRepositoryDecorator<>).MakeGenericType(aggregateRootType),
            //                sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType)))
            //            ?? throw new InvalidOperationException($"Couldn't create EventAddingRepositoryDecorator for aggregateRootType {aggregateRootType.Name}"));
            //    }

            return services;
        }
    }
}
