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

        public LocalSettings Build()
        {
            var data = new LocalSettings();

            data.DbConnectionSettings.DBMS = Enum.Parse<EnumDataBaseMS>((_dbSettings[$"Database-{_environmentType}:DBMS"] ?? "Null"));
            data.DbConnectionSettings.Host = _dbSettings[$"Database-{_environmentType}:Host"] ?? "";
            data.DbConnectionSettings.Port = int.Parse(_dbSettings[$"Database-{_environmentType}:Port"] ?? "0");
            data.DbConnectionSettings.DatabaseName = _dbSettings[$"Database-{_environmentType}:DatabaseName"] ?? "";
            data.DbConnectionSettings.Username = _dbSettings[$"Database-{_environmentType}:Username"] ?? "";
            data.DbConnectionSettings.Password = _dbSettings[$"Database-{_environmentType}:Password"] ?? "";

            // TODO When translating ArchivePath turn % into #
            // TODO ArchiveAboveSize Convert Mb to Bytes ( * 1048576 )

            return data;

        }



    }
}
