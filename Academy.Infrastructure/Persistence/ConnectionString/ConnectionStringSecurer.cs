using Academy.Infrastructure.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Npgsql;

namespace Academy.Infrastructure.Persistence.ConnectionString
{
    public class ConnectionStringSecurer : IConnectionStringSecurer
    {
        private const string HiddenValueDefault = "*******";
        private readonly DatabaseSettings _dbSettings;

        public ConnectionStringSecurer(IOptions<DatabaseSettings> dbSettings) =>
            _dbSettings = dbSettings.Value;

        public string? MakeSecure(string? connectionString, string? dbProvider)
        {
            if (connectionString == null || string.IsNullOrEmpty(connectionString))
            {
                return connectionString;
            }

            if (string.IsNullOrWhiteSpace(dbProvider))
            {
                dbProvider = _dbSettings.DBProvider;
            }

            return dbProvider?.ToLower() switch
            {
                DbProviderKeys.Npgsql => MakeSecureNpgsqlConnectionString(connectionString),
                DbProviderKeys.SqlServer => MakeSecureSqlConnectionString(connectionString),
                DbProviderKeys.MySql => MakeSecureMySqlConnectionString(connectionString),
                _ => connectionString
            };
        }

        private static string MakeSecureMySqlConnectionString(string connectionString)
        {
            var builder = new MySqlConnectionStringBuilder(connectionString);

            if (!string.IsNullOrEmpty(builder.Password))
            {
                builder.Password = HiddenValueDefault;
            }

            if (!string.IsNullOrEmpty(builder.UserID))
            {
                builder.UserID = HiddenValueDefault;
            }

            return builder.ToString();
        }

        private static string MakeSecureSqlConnectionString(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);

            if (!string.IsNullOrEmpty(builder.Password) || !builder.IntegratedSecurity)
            {
                builder.Password = HiddenValueDefault;
            }

            if (!string.IsNullOrEmpty(builder.UserID) || !builder.IntegratedSecurity)
            {
                builder.UserID = HiddenValueDefault;
            }

            return builder.ToString();
        }

        private static string MakeSecureNpgsqlConnectionString(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);

            if (!string.IsNullOrEmpty(builder.Password))
            {
                builder.Password = HiddenValueDefault;
            }

            if (!string.IsNullOrEmpty(builder.Username))
            {
                builder.Username = HiddenValueDefault;
            }

            return builder.ToString();
        }
    }
}