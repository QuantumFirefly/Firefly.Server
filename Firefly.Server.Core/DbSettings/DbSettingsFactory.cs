using Microsoft.Extensions.Configuration;
using Firefly.Server.Core.Entitys;

namespace Firefly.Server.Core.DbSettings
{
    public class DbSettingsFactory
    {

        private string _environmentType;
        private IConfigurationRoot _dbSettings;
        public DbSettingsFactory(string IniFileName, string EnvironmentType = "Production") {
            _dbSettings = new ConfigurationBuilder()
            .AddIniFile(IniFileName, optional: false, reloadOnChange: true)
            .Build();
            _environmentType = EnvironmentType;
        }// test 123 456

        public DbConnectionSettings Build()
        {
            try
            {
                var data = new Entitys.DbConnectionSettings();

                var dbmsFullName = _dbSettings[$"{_environmentType}:DBMS"];
                data.Host = _dbSettings[$"{_environmentType}:Host"];
                data.Port = int.Parse((_dbSettings[$"{_environmentType}:Port"]));
                data.Username = _dbSettings[$"{_environmentType}:Username"];
                data.Password = _dbSettings[$"{_environmentType}:Password"];

                return data;

            } catch (Exception ex) {
                // Log the failure here.
                return null;
            }
            
        }



    }
}
