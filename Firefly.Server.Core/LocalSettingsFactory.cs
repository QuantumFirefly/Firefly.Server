using Microsoft.Extensions.Configuration;
using Firefly.Server.Core.Entitys;
using Firefly.Server.Core.Enums;

namespace Firefly.Server.Core
{
    public class LocalSettingsFactory
    {

        private string _environmentType;
        private IConfigurationRoot _dbSettings;
        public LocalSettingsFactory(string IniFileName, string EnvironmentType = "Production")
        {
            _dbSettings = new ConfigurationBuilder()
            .AddIniFile(IniFileName, optional: false, reloadOnChange: true)
            .Build();
            _environmentType = EnvironmentType;
        }

        public DbConnectionSettings Build()
        {
            var data = new DbConnectionSettings();

            data.DBMS = Enum.Parse<EnumDataBaseMS>((_dbSettings[$"Database-{_environmentType}:DBMS"] ?? "Null"));
            data.Host = _dbSettings[$"Database-{_environmentType}:Host"] ?? "";
            data.Port = int.Parse(_dbSettings[$"Database-{_environmentType}:Port"] ?? "0");
            data.Username = _dbSettings[$"Database-{_environmentType}:Username"] ?? "";
            data.Password = _dbSettings[$"Database-{_environmentType}:Password"] ?? "";

            return data;

        }



    }
}
