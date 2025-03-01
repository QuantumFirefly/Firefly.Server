using Microsoft.Extensions.Configuration;
using Firefly.Server.Core.Entitys;

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

            var dbmsFullName = _dbSettings[$"{_environmentType}:DBMS"]; // TODO Convert to Enum
            data.Host = _dbSettings[$"{_environmentType}:Host"];
            data.Port = int.Parse(_dbSettings[$"{_environmentType}:Port"]);
            data.Username = _dbSettings[$"{_environmentType}:Username"];
            data.Password = _dbSettings[$"{_environmentType}:Password"];

            return data;

        }



    }
}
