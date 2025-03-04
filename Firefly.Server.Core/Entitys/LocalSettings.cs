using Firefly.Server.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Entitys
{
    public class LocalSettings : ISettings
    {
        public DbConnectionSettings DbConnectionSettings { get; set; } = new DbConnectionSettings();
        public LogSettings LogSettings { get; set; } = new LogSettings();

        public bool Validate(ref List<string> messages) {
            bool validationPassed = true;

            if (!DbConnectionSettings.Validate(ref messages)) validationPassed = false;
            if (!LogSettings.Validate(ref messages)) validationPassed = false;

            return validationPassed;
        }

        public static LocalSettings Build(string iniFileName, string dbEnvironmentType) {
            IConfigurationRoot iniContent = new ConfigurationBuilder()
            .AddIniFile(iniFileName, optional: false, reloadOnChange: true)
            .Build();

            var data = new LocalSettings();
            data.DbConnectionSettings = DbConnectionSettings.Build(iniContent, dbEnvironmentType);
            data.LogSettings = LogSettings.Build(iniContent);
            return data;
        }

    }
}
