using Firefly.Server.Core.Enums;
using Npgsql;
using Dapper;
using System.Text.Json;
using Firefly.Server.Core.Entities.LocalConfig;
using NLog;
using System.Data;

namespace Firefly.Server.Core.Database
{
    public class DbConnection : IDbConnection
    {

        public readonly EnumDBMS DBMS;
        private NpgsqlConnection _connection;
        private readonly ILogger _log;
        private readonly string _connectionString;
        public DbConnection(DbConnectionConfig? connectionSettings, ILogger log) {
            ArgumentNullException.ThrowIfNull(connectionSettings);

            if (connectionSettings.DBMS != Enums.EnumDBMS.PostgreSQL)
            {
                throw new ArgumentException("Only PostgreSQL is supported as an DBMS.");
            }
            DBMS = connectionSettings.DBMS;
            _connectionString = connectionSettings.ToConnectionString;
            _connection = new NpgsqlConnection(_connectionString);

            _log = log;
            _log.Log(LogLevel.Trace, $"Opening DB Connection...");
            _connection.Open();
            _log.Log(LogLevel.Debug, $"Database Connected Opened.");
        }

        public async Task<T?> JsonGetAsync<T>(string query) {
            KeepAlive();
            var jsonResult = await _connection.QuerySingleOrDefaultAsync<string>(query) ?? "";

            return JsonSerializer.Deserialize<T>(jsonResult);
        }

        public void Dispose() {
            GC.SuppressFinalize(this);
            _connection.Close();
            _connection?.Dispose();
            _log.Log(LogLevel.Trace, $"DBConn Disposed.");
        }
        ~DbConnection() {
            Dispose(); // Just incase Dispose() is not called!
        }

        private void KeepAlive() {
            try {
                if (_connection == null || _connection.State != ConnectionState.Open) {
                    _log.Log(LogLevel.Warn, "Reconnecting to the database...");
                    _connection?.Dispose();
                    _connection = new NpgsqlConnection(_connectionString);
                    _connection.Open();
                    _log.Log(LogLevel.Debug, "Database Connection Reestablished.");
                }
            } catch (Exception ex) {
                _log.Log(LogLevel.Error, $"Database Connection Failed: {ex.Message}");
                throw;
            }
        }
    }
}
