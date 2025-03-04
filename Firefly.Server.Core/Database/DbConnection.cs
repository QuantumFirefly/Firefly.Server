using Firefly.Server.Core.Entitys;
using Npgsql;

namespace Firefly.Server.Core.Database
{
    public class DbConnection
    {
        private NpgsqlConnection _connection;
        public DbConnection(DbConnectionSettings connectionSettings) {
            if (connectionSettings.DBMS != Enums.EnumDataBaseMS.PostgreSQL)
            {
                throw new ArgumentException("Only PostgreSQL is supported as an DBMS.");
            }
            _connection = new NpgsqlConnection(connectionSettings.ToConnectionString);
        }

        // TODO - Create mapping functions for Scaler, Query & Non-Query. (Non-query can take in array of tuples for paramters)
    }
}
