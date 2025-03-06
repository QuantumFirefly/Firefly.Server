using Firefly.Server.Core.Enums;
using Firefly.Server.Core.LocalConfig;
using Npgsql;

namespace Firefly.Server.Core.Database
{
    public class DbConnection : IDisposable
    {

        public readonly EnumDBMS DBMS;
        private readonly NpgsqlConnection _connection;
        public DbConnection(DbConnectionConfig connectionSettings) {
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

        // TODO - Create mapping functions for Scaler, Query & Non-Query. (Non-query can take in array of tuples for paramters)
        // TODO - Querys should hit database async

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
