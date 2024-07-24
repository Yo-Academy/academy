using Academy.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.PostgreSQL;
using System.Collections.ObjectModel;
using ColumnOptionsSqlServer = Serilog.Sinks.MSSqlServer;

namespace Academy.Infrastructure.Logging.Serilog
{
    public static class Extensions
    {
        public static WebApplicationBuilder RegisterSerilog(this WebApplicationBuilder builder)
        {
            builder.Services.AddOptions<LoggerSettings>().BindConfiguration(nameof(LoggerSettings));
            builder.Host.UseSerilog((_, sp, serilogConfig) =>
            {
                var loggerSettings = sp.GetRequiredService<IOptions<LoggerSettings>>().Value;
                var databaseSettings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                ConfigureEnrichers(serilogConfig, loggerSettings.AppName, builder.Services);
                ConfigureConsoleLogging(serilogConfig, loggerSettings.StructuredConsoleLogging, loggerSettings.WriteToConsole);
                ConfigureWriteToFile(serilogConfig, loggerSettings.WriteToFile);
                ConfigureElasticSearch(builder, serilogConfig, loggerSettings.AppName, loggerSettings.ElasticSearchUrl);
                ConfigureDatabaseLogging(serilogConfig, databaseSettings.ConnectionString, databaseSettings.DBProvider);
                SetMinimumLogLevel(serilogConfig, loggerSettings.MinimumLogLevel);
                OverideMinimumLogLevel(serilogConfig);
            });
            return builder;
        }

        private static void ConfigureEnrichers(LoggerConfiguration serilogConfig, string appName, IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

            serilogConfig
                        .Enrich.FromLogContext()
                        .Enrich.With(new CustomLogEventEnricher(httpContextAccessor));
        }

        private static void ConfigureConsoleLogging(LoggerConfiguration serilogConfig, bool structuredConsoleLogging, bool writeToConsole)
        {
            if (writeToConsole)
            {
                if (structuredConsoleLogging)
                {
                    serilogConfig.WriteTo.Async(wt => wt.Console(new CompactJsonFormatter()));
                }
                else
                {
                    serilogConfig.WriteTo.Async(wt => wt.Console());
                }
            }
        }

        private static void ConfigureWriteToFile(LoggerConfiguration serilogConfig, bool writeToFile)
        {
            if (writeToFile)
            {
                serilogConfig.WriteTo.File(
                 new CompactJsonFormatter(),
                 "Logs/logs.json",
                 restrictedToMinimumLevel: LogEventLevel.Error,
                 rollingInterval: RollingInterval.Day,
                 retainedFileCountLimit: 5);
            }
        }
        private static void ConfigureElasticSearch(WebApplicationBuilder builder, LoggerConfiguration serilogConfig, string appName, string elasticSearchUrl)
        {
            if (!string.IsNullOrEmpty(elasticSearchUrl))
            {
                string? formattedAppName = appName?.ToLower().Replace(".", "-").Replace(" ", "-");
                string indexFormat = $"{formattedAppName}-logs-{builder.Environment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}";
                serilogConfig.WriteTo.Async(writeTo =>
                writeTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = indexFormat,
                    MinimumLogEventLevel = LogEventLevel.Information,
                    //ModifyConnectionSettings = conn =>
                    //{
                    //    return conn.BasicAuthentication("elastic", "KDywMsZp5Hi5+xK05EV=");
                    //}
                })).Enrich.WithProperty("Environment", builder.Environment.EnvironmentName!);
            }
        }

        private static void ConfigureDatabaseLogging(LoggerConfiguration serilogConfig, string connectionString, string dBProvider)
        {
            switch (dBProvider)
            {
                case "postgresql":

                    var customColumnWriters = new Dictionary<string, ColumnWriterBase>
                    {
                        {"Id", new SinglePropertyColumnWriter("Id", PropertyWriteMethod.Raw,NpgsqlDbType.Uuid) },
                        {"UserId", new SinglePropertyColumnWriter("UserId", PropertyWriteMethod.Raw,NpgsqlDbType.Uuid) },
                        {"TenantId", new SinglePropertyColumnWriter("TenantId", PropertyWriteMethod.Raw,NpgsqlDbType.Varchar,columnLength: 64) },
                        {"Message", new RenderedMessageColumnWriter() },
                        {"RequestPath", new SinglePropertyColumnWriter("RequestPath", PropertyWriteMethod.Raw,NpgsqlDbType.Text) },
                        {"RequestMethod", new SinglePropertyColumnWriter("RequestMethod", PropertyWriteMethod.Raw,NpgsqlDbType.Text) },
                        {"RequestHeaders", new SinglePropertyColumnWriter("RequestHeaders", PropertyWriteMethod.Raw,NpgsqlDbType.Text) },
                        {"RequestBody", new SinglePropertyColumnWriter("RequestBody", PropertyWriteMethod.Raw,NpgsqlDbType.Text) },
                        {"Level", new LevelColumnWriter(true , NpgsqlDbType.Varchar)},
                        {"ExecutionTime", new SinglePropertyColumnWriter("ExecutionTime", PropertyWriteMethod.Raw,NpgsqlDbType.Timestamp) },
                        {"ExecutionDuration", new SinglePropertyColumnWriter("ExecutionDuration", PropertyWriteMethod.Raw,NpgsqlDbType.Bigint) },
                        {"ErrorId", new SinglePropertyColumnWriter("ErrorId", PropertyWriteMethod.Raw,NpgsqlDbType.Uuid) },
                        {"Exception", new ExceptionColumnWriter() },
                        {"StatusCode", new SinglePropertyColumnWriter("StatusCode", PropertyWriteMethod.Raw,NpgsqlDbType.Integer) },
                        {"SourceContext", new SinglePropertyColumnWriter("SourceContext", PropertyWriteMethod.Raw,NpgsqlDbType.Text) },
                        {"ClientIpAddress", new SinglePropertyColumnWriter("ClientIpAddress", PropertyWriteMethod.Raw,NpgsqlDbType.Text) },
                        {"BrowserInfo", new SinglePropertyColumnWriter("BrowserInfo", PropertyWriteMethod.Raw,NpgsqlDbType.Text) }
                    };
                    serilogConfig
                        .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error && evt.Level == LogEventLevel.Information))
                        .WriteTo.PostgreSQL(
                            connectionString,
                            "AuditLogs",
                            columnOptions: customColumnWriters,
                            needAutoCreateTable: true,
                            restrictedToMinimumLevel: LogEventLevel.Information,
                            respectCase: true
                        );
                    break;

                case "mssql":
                    var columnOptions = new ColumnOptionsSqlServer.ColumnOptions();
                    columnOptions.Store.Remove(StandardColumn.Id);
                    columnOptions.Store.Remove(StandardColumn.MessageTemplate);
                    columnOptions.Store.Remove(StandardColumn.TraceId);
                    columnOptions.Store.Remove(StandardColumn.SpanId);
                    columnOptions.Store.Remove(StandardColumn.Properties);
                    columnOptions.Store.Remove(StandardColumn.LogEvent);
                    columnOptions.Store.Remove(StandardColumn.TimeStamp);
                    columnOptions.AdditionalColumns = new Collection<SqlColumn>
                    {
                        new SqlColumn { ColumnName = "Id", DataType = System.Data.SqlDbType.UniqueIdentifier},
                        new SqlColumn {ColumnName = "UserId", DataType = System.Data.SqlDbType.UniqueIdentifier},
                        new SqlColumn {ColumnName = "TenantId", DataType = System.Data.SqlDbType.VarChar},
                        new SqlColumn {ColumnName = "RequestPath", DataType = System.Data.SqlDbType.NVarChar},
                        new SqlColumn {ColumnName = "RequestMethod", DataType = System.Data.SqlDbType.NVarChar},
                        new SqlColumn {ColumnName = "RequestHeaders", DataType = System.Data.SqlDbType.NVarChar},
                        new SqlColumn {ColumnName = "RequestBody", DataType = System.Data.SqlDbType.NVarChar},
                        new SqlColumn {ColumnName = "ExecutionTime", DataType = System.Data.SqlDbType.DateTime},
                        new SqlColumn {ColumnName = "ExecutionDuration", DataType = System.Data.SqlDbType.BigInt},
                        new SqlColumn {ColumnName = "ErrorId", DataType = System.Data.SqlDbType.UniqueIdentifier},
                        new SqlColumn {ColumnName = "StatusCode", DataType = System.Data.SqlDbType.Int},
                        new SqlColumn {ColumnName = "SourceContext", DataType = System.Data.SqlDbType.VarChar},
                        new SqlColumn {ColumnName = "ClientIpAddress", DataType = System.Data.SqlDbType.NVarChar},
                        new SqlColumn {ColumnName = "BrowserInfo", DataType = System.Data.SqlDbType.NVarChar}
                    };


                    serilogConfig
                        .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error && evt.Level == LogEventLevel.Information))
                        .WriteTo.MSSqlServer(
                        connectionString,
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        sinkOptions: new MSSqlServerSinkOptions
                        {
                            TableName = "AuditLogs",
                            AutoCreateSqlTable = true
                        },
                        columnOptions: columnOptions);
                    break;
                case "mysql":

                    serilogConfig.WriteTo.MySQL(
                        connectionString,
                        "AuditLogs",
                        restrictedToMinimumLevel: LogEventLevel.Information);
                    break;
                default:
                    break;

            }

        }

        private static void OverideMinimumLogLevel(LoggerConfiguration serilogConfig)
        {
            serilogConfig
                         .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                         .MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
                         .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                         .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error);
        }

        private static void SetMinimumLogLevel(LoggerConfiguration serilogConfig, string minLogLevel)
        {
            switch (minLogLevel.ToLower())
            {
                case "debug":
                    serilogConfig.MinimumLevel.Debug();
                    break;
                case "information":
                    serilogConfig.MinimumLevel.Information();
                    break;
                case "warning":
                    serilogConfig.MinimumLevel.Warning();
                    break;
                default:
                    serilogConfig.MinimumLevel.Error();
                    break;
            }
        }

        private static IDictionary<string, ColumnWriterBase> GenerateCustomColumnWriters<T>()
        {
            var customColumnWriters = new Dictionary<string, ColumnWriterBase>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var columnName = property.Name;
                var propertyType = property.PropertyType;
                var columnWriter = GetColumnWriter(columnName, propertyType);
                customColumnWriters.Add(columnName, columnWriter);
            }

            return customColumnWriters;
        }

        private static ColumnWriterBase GetColumnWriter(string columnName, Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                return new SinglePropertyColumnWriter(columnName, PropertyWriteMethod.ToString, NpgsqlDbType.Varchar);
            }
            else if (propertyType == typeof(int))
            {
                return new SinglePropertyColumnWriter(columnName, PropertyWriteMethod.Raw, NpgsqlDbType.Integer);
            }
            else if (propertyType == typeof(DateTime))
            {
                return new SinglePropertyColumnWriter(columnName, PropertyWriteMethod.Raw, NpgsqlDbType.Timestamp);
            }
            // Add more type mappings as needed
            else
            {
                throw new NotSupportedException($"Unsupported property type: {propertyType.Name}");
            }
        }
    }
}
