using Firefly.Server.Core.Enums;

namespace Firefly.Server.Core.Entitys
{
    public class DbConnectionSettings
    {

        public EnumDatabaseMS DBMS { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Database { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ConnectionString => $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};";
    }
}
