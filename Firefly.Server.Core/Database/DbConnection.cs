using Firefly.Server.Core.Enums;
using Npgsql;
using Dapper;
using System.Text.Json;
using Firefly.Server.Core.Entities.LocalConfig;

namespace Firefly.Server.Core.Database
{
    public class DbConnection : IDisposable
    {

        public readonly EnumDBMS DBMS;
        private readonly NpgsqlConnection _connection;
        public DbConnection(DbConnectionConfig? connectionSettings) {
            ArgumentNullException.ThrowIfNull(connectionSettings);

            if (connectionSettings.DBMS != Enums.EnumDBMS.PostgreSQL)
            {
                throw new ArgumentException("Only PostgreSQL is supported as an DBMS.");
            }
            DBMS = connectionSettings.DBMS;
            _connection = new NpgsqlConnection(connectionSettings.ToConnectionString);
        }

        public void Open() {
            _connection.Open();
        }

        public async Task<T?> JsonGet<T>(string query) {
            var jsonResult = await _connection.QuerySingleOrDefaultAsync<string>(query) ?? "";

            return JsonSerializer.Deserialize<T>(jsonResult);
        }

        public void Dispose() {
            GC.SuppressFinalize(this);
            _connection.Close();
            _connection?.Dispose();
        }
        ~DbConnection() {
            Dispose(); // Just incase Dispose() is not called!
        }
    }
}
