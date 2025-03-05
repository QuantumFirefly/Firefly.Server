using Firefly.Server.Core;
using Firefly.Server.Core.Database;
using Firefly.Server.Core.LocalConfig;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Worker
{


    internal class MasterWorker
    {

        ILogger _log;
        public MasterWorker(ILogger? logService = null) {
            _log = logService ?? LogManager.GetCurrentClassLogger();
        }

        public async Task Start() {
            var version = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;

            Console.WriteLine($"Firefly Server v{version} Booting up...");

            Console.WriteLine($"Reading {Constants.LOCAL_SETTINGS_INI_FILE}...");

            LocalConfig localSettings;
            if (!_importValidateAndApplyLocalSettings(out localSettings, Constants.LOCAL_SETTINGS_INI_FILE, Constants.DB_ENVIRONMENT_TYPE)) {
                return;
            }

            _log.Log(LogLevel.Info, $"Firefly Server v{version} - Local Settings Imported & Validated.");

            _log.Log(LogLevel.Info, $"Connecting to {localSettings.DbConnectionSettings.DBMS} Database {localSettings.DbConnectionSettings.Host}:{localSettings.DbConnectionSettings.Port}...");
            using (var dbConnection = new DbConnection(localSettings.DbConnectionSettings)) {
                try {
                    dbConnection.Open();
                } catch (Exception ex) {
                    _log.Log(LogLevel.Fatal, $"{ex.Message}");
                    return;
                }
                
                _log.Log(LogLevel.Debug, $"Database Connected!");
            }
            
            // TODO - Provide tool that takes db info from Local Settings. Takes super user password, and creates DB & user/pass
            
            /* TODO
             * Connect to Database. Test can run some querys. Ensure Encryption is on.
             * --> DBconnection object to be passed into Repos via DI.
             * Upgrade to latest version if needed (Creating tables, alter statements, etc). Is there a library I can use to do this? (EF Migrations? Might be far too verbose)
             * Import RemoteSettings from database table. Setup repo & queries class for this. (need an appropiate name, FireFlySettings? Probably better to have a parent calss also that has both local & remote settings)
             * Create IRC Settings - and class hierachy for it.
             * Start to listen in on IRC port for inbound TCP connections. Start new worker thread for each connection.
             * 
             * Integration & Unit Tests
             */


        }

        private bool _importValidateAndApplyLocalSettings(out LocalConfig localSettings, string iniFile, string dbEnvironmentType) {
            try {
                localSettings = LocalConfig.Build(iniFile, dbEnvironmentType);

                var messages = new List<String>();
                if (!localSettings.Validate(ref messages)) {
                    var exMsg = string.Join("; ", messages);
                    throw new Exception(exMsg);
                }
            } catch (Exception ex) {
                Console.WriteLine($"ERROR: Unable to read from {iniFile}. {ex.ToString()}.");
                localSettings = null;
                return false;
            }

            try {
                LogConfig.ApplySettingsToNLog(localSettings.LogSettings);
            } catch (Exception ex) {
                Console.WriteLine($"ERROR: Error applying config to NLog. {ex.ToString()}.");
                localSettings = null;
                return false;
            }
            
            return true;
        }
    }
}
