using Firefly.Server.Core.Enums;
using Microsoft.Extensions.Configuration;

namespace Firefly.Server.Core.LocalConfig
{
    public class DbConnectionConfig : IConfig
    {

        public EnumDataBaseMS DBMS { get; set; }

        public string Host { get; set; } = "";

        public int Port { get; set; } = 0;

        public string DatabaseName { get; set; } = "";

        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public string ToConnectionString => $"Host={Host};Port={Port};Database={DatabaseName};Username={Username};Password={Password};";

        public bool Validate(ref List<string> messages) {
            bool validationPassed = true;

            if (DBMS != EnumDataBaseMS.PostgreSQL) {
                messages.Add("Only PostgreSQL is supported as a valid DBMS.");
                validationPassed = false;
            }

            if (Host.Length <= 1) {
                messages.Add($"Database Host must be more than 1 character! Current length is {Host.Length}");
                validationPassed = false;
            }

            if (Port < Constants.LOWEST_PORT_ALLOWED || Port > Constants.HIGHEST_PORT_ALLOWED) {
                messages.Add($"Database Port must be no lower than {Constants.LOWEST_PORT_ALLOWED} and no higher than {Constants.HIGHEST_PORT_ALLOWED}. Current value is {Port}");
                validationPassed = false;
            }

            if (DatabaseName.Length <= 1) {
                messages.Add($"Database Name must be more than 1 character! Current length is {DatabaseName.Length}");
                validationPassed = false;
            }

            if (Username.Length <= 1) {
                messages.Add($"Database Username must be more than 1 character! Current length is {Username.Length}");
                validationPassed = false;
            }

            if (Password.Length <= 1) {
                messages.Add($"Database Password must be more than 1 character! Current length is {Password.Length}");
                validationPassed = false;
            }

            return validationPassed;
        }

        public static DbConnectionConfig Build(IConfigurationRoot iniContent, string dbEnvironmentType) {
            var data = new DbConnectionConfig{
                DBMS = Enum.Parse<EnumDataBaseMS>(iniContent[$"Database-{dbEnvironmentType}:DBMS"] ?? "Null"),
                Host = iniContent[$"Database-{dbEnvironmentType}:Host"] ?? "",
                Port = int.Parse(iniContent[$"Database-{dbEnvironmentType}:Port"] ?? "-1"),
                DatabaseName = iniContent[$"Database-{dbEnvironmentType}:DatabaseName"] ?? "",
                Username = iniContent[$"Database-{dbEnvironmentType}:Username"] ?? "",
                Password = iniContent[$"Database-{dbEnvironmentType}:Password"] ?? ""
            };
            return data;
        }
    }
}
