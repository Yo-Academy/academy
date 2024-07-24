using Academy.Infrastructure.Common;
using Academy.Infrastructure.Persistence;
using Community.Microsoft.Extensions.Caching.PostgreSql;
using Microsoft.Extensions.Configuration;

namespace Academy.Infrastructure.Caching
{
    internal static class CatchingServiceRegistration
    {
        internal static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration config)
        {
            var settings = config.GetSection(nameof(CacheSettings)).Get<CacheSettings>();
            if (settings == null) return services;
            if (settings.UseDistributedCache)
            {
                if (settings.PreferRedis)
                {
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = settings.RedisURL;
                        options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                        {
                            AbortOnConnectFail = true,
                            EndPoints = { settings.RedisURL }
                        };
                    });
                }
                else
                {
                    if (settings.PreferDatabase)
                    {
                        var databaseSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
                        switch (databaseSettings.DBProvider?.ToLowerInvariant())
                        {
                            case DbProviderKeys.Npgsql:
                                services.AddDistributedPostgreSqlCache(options =>
                                {
                                    options.ConnectionString = databaseSettings.ConnectionString;
                                    options.SchemaName = "public";
                                    options.TableName = "CachedData";
                                });
                                break;

                            case DbProviderKeys.MySql:
                                services.AddDistributedMySqlCache(options =>
                                {
                                    options.ConnectionString = databaseSettings.ConnectionString;
                                    options.SchemaName = "dbo";
                                    options.TableName = "CachedData";
                                });
                                break;

                            case DbProviderKeys.SqlServer:
                                services.AddDistributedSqlServerCache(options =>
                                {
                                    options.ConnectionString = databaseSettings.ConnectionString;
                                    options.SchemaName = "dbo";
                                    options.TableName = "CachedData";
                                });
                                break;
                        }
                    }
                    else
                        services.AddDistributedMemoryCache();
                }

                services.AddTransient<ICacheService, DistributedCacheService>();
            }
            else
            {
                services.AddTransient<ICacheService, LocalCacheService>();
            }

            services.AddMemoryCache();
            return services;
        }
    }
}