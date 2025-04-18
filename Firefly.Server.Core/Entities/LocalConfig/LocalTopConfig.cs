﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Entities.LocalConfig
{
    public class LocalTopConfig : IConfig
    {
        public DbConnectionConfig? DbConnectionSettings { get; set; }
        public LogConfig? LogSettings { get; set; }

        public bool Validate(ref List<string> messages) {
            bool validationPassed = true;

            if (DbConnectionSettings != null && !DbConnectionSettings.Validate(ref messages)) validationPassed = false;
            if (LogSettings != null && !LogSettings.Validate(ref messages)) validationPassed = false;

            if (DbConnectionSettings == null) {
                validationPassed = false;
                messages.Add("DbConnectionSettings Missing from ini file.");
            }

            if (LogSettings == null) {
                validationPassed = false;
                messages.Add("LogSettings Missing from ini file.");
            }

            return validationPassed;
        }

        public static LocalTopConfig Build(string iniFileName, string dbEnvironmentType) {
            IConfigurationRoot iniContent = new ConfigurationBuilder()
            .AddIniFile(iniFileName, optional: false, reloadOnChange: true)
            .Build();

            var data = new LocalTopConfig {
                DbConnectionSettings = DbConnectionConfig.Build(iniContent, dbEnvironmentType),
                LogSettings = LogConfig.Build(iniContent)
            };
            return data;
        }

    }
}
