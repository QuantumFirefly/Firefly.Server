﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.LocalConfig
{
    public class LocalConfig : IConfig
    {
        public DbConnectionConfig DbConnectionSettings { get; set; } = new DbConnectionConfig();
        public LogConfig LogSettings { get; set; } = new LogConfig();

        public bool Validate(ref List<string> messages) {
            bool validationPassed = true;

            if (!DbConnectionSettings.Validate(ref messages)) validationPassed = false;
            if (!LogSettings.Validate(ref messages)) validationPassed = false;

            return validationPassed;
        }

        public static LocalConfig Build(string iniFileName, string dbEnvironmentType) {
            IConfigurationRoot iniContent = new ConfigurationBuilder()
            .AddIniFile(iniFileName, optional: false, reloadOnChange: true)
            .Build();

            var data = new LocalConfig();
            data.DbConnectionSettings = DbConnectionConfig.Build(iniContent, dbEnvironmentType);
            data.LogSettings = LogConfig.Build(iniContent);
            return data;
        }

    }
}
