using Microsoft.Extensions.Configuration;
using Firefly.Server.Core.Entitys;
using Firefly.Server.Core.Enums;
using NLog;
using NLog.Targets;

namespace Firefly.Server.Core
{
    public class LocalSettingsFactory
    {

        private string _environmentType;
        private IConfigurationRoot _dbSettings;
        public LocalSettingsFactory(string IniFileName, string EnvironmentType = "Production") {
            _dbSettings = new ConfigurationBuilder()
            .AddIniFile(IniFileName, optional: false, reloadOnChange: true)
            .Build();
            _environmentType = EnvironmentType;
        }

        public LocalSettings Build() {
            var data = new LocalSettings();

            data.DbConnectionSettings.DBMS = Enum.Parse<EnumDataBaseMS>((_dbSettings[$"Database-{_environmentType}:DBMS"] ?? "Null"));
            data.DbConnectionSettings.Host = _dbSettings[$"Database-{_environmentType}:Host"] ?? "";
            data.DbConnectionSettings.Port = int.Parse(_dbSettings[$"Database-{_environmentType}:Port"] ?? "-1");
            data.DbConnectionSettings.DatabaseName = _dbSettings[$"Database-{_environmentType}:DatabaseName"] ?? "";
            data.DbConnectionSettings.Username = _dbSettings[$"Database-{_environmentType}:Username"] ?? "";
            data.DbConnectionSettings.Password = _dbSettings[$"Database-{_environmentType}:Password"] ?? "";

            data.LogSettings.logLevel = LogLevel.FromString(_dbSettings[$"Logging:LogLevel"] ?? "Off");
            data.LogSettings.target = _dbSettings[$"Logging:Target"] ?? "";
            data.LogSettings.filePath = _dbSettings[$"Logging:FilePath"] ?? "";
            data.LogSettings.archiveEvery = Enum.Parse<FileArchivePeriod>(_dbSettings[$"Logging:ArchiveEvery"] ?? "None");
            data.LogSettings.archiveNumbering = Enum.Parse<ArchiveNumberingMode>(_dbSettings[$"Logging:ArchiveNumbering"] ?? "");
            data.LogSettings.archivePath = _dbSettings[$"Logging:ArchivePath"] ?? "";
            data.LogSettings.maxArchiveFiles = int.Parse(_dbSettings[$"Logging:MaxArchiveFiles"] ?? "-1");
            data.LogSettings.archiveAboveSize = long.Parse(_dbSettings[$"Logging:ArchiveAboveSize"] ?? "-1");
            data.LogSettings.archiveDateFormat = _dbSettings[$"Logging:ArchiveDateFormat"] ?? "";

            return data;

        }
    }
}
