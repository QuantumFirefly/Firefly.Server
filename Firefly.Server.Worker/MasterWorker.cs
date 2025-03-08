using Firefly.Server.Core;
using Firefly.Server.Core.Database;
using Firefly.Server.Core.Database.Repository;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Firefly.Server.Core.Entities.RemoteConfig;
using Firefly.Server.Core.Entities.LocalConfig;
using Firefly.Server.Core.Entities;
using System.Data.Common;
using DbConnection = Firefly.Server.Core.Database.DbConnection;


namespace Firefly.Server.Worker
{


    internal class MasterWorker(ILogger? logService = null)
    {

        private readonly ILogger _log = logService ?? LogManager.GetCurrentClassLogger();

        public void Start() {
            try {
                var version = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;

                Console.WriteLine($"Firefly Server v{version} Booting up...");

                Console.WriteLine($"Reading {Constants.LOCAL_SETTINGS_INI_FILE}...");

                if (!ImportValidateAndApplyLocalSettings(out LocalTopConfig localConfig, Constants.LOCAL_SETTINGS_INI_FILE, Constants.DB_ENVIRONMENT_TYPE)) {
                    return;
                }

                _log.Log(LogLevel.Info, $"Firefly Server v{version} - Local Settings Imported & Validated.");

                if (localConfig.DbConnectionSettings == null) throw new Exception($"Database Connection details missing from {Constants.LOCAL_SETTINGS_INI_FILE}. Environment type: {Constants.DB_ENVIRONMENT_TYPE}");
                var migrator = new Migrations(localConfig.DbConnectionSettings.ToConnectionString, _log);

                _log.Log(LogLevel.Info, $"Checking if {localConfig.DbConnectionSettings?.DBMS} Database {localConfig.DbConnectionSettings?.DatabaseName} Exists on {localConfig.DbConnectionSettings?.Host}:{localConfig.DbConnectionSettings?.Port}...");
                migrator.CreateDbIfNotExists();

                _log.Log(LogLevel.Info, $"Running migrations...");
                migrator.RunMigrations();


                _log.Log(LogLevel.Info, $"Connecting to {localConfig.DbConnectionSettings?.DBMS} Database {localConfig.DbConnectionSettings?.DatabaseName} {localConfig.DbConnectionSettings?.Host}:{localConfig.DbConnectionSettings?.Port}...");
            
                using var dbConnection = new DbConnection(localConfig.DbConnectionSettings);
                dbConnection.Open();

                _log.Log(LogLevel.Debug, $"Database Connected!");

                _log.Log(LogLevel.Info, $"Loading Firefly Remote Config from Database...");
                if (!ImportandValidateRemoteSettings(out FireflyConfig fireflyConfig, localConfig, dbConnection, _log)) {
                    return;
                }



            } catch (Exception ex) {
                _log.Log(LogLevel.Fatal, $"{ex.Message}");
                return;
            }
            
            /* TODO   
             * Start to listen in on IRC port for inbound TCP connections.
             * Classes need abstracting to Interfaces for DI & Unit Testing.
             * 
             * Integration & Unit Tests
             */
        }

        private static bool ImportandValidateRemoteSettings(out FireflyConfig fireflyConfig, LocalTopConfig localConfig, DbConnection db, ILogger log) {
            fireflyConfig = new FireflyConfig {
                Local = localConfig,
                Remote = new ConfigRepo(db).GetAll().Result
            };

            log.Log(LogLevel.Debug, $"Validating Firefly Remote Config...");
            List<string> messages = [];
            if (!fireflyConfig.Remote.Validate(ref messages)) {
                var exMsg = string.Join("; ", messages);
                throw new Exception(exMsg);
            }

            return true;
        }

        private static bool ImportValidateAndApplyLocalSettings(out LocalTopConfig localConfig, string iniFile, string dbEnvironmentType) {
            try {
                localConfig = LocalTopConfig.Build(iniFile, dbEnvironmentType);

                var messages = new List<String>();
                if (!localConfig.Validate(ref messages)) {
                    var exMsg = string.Join("; ", messages);
                    throw new Exception(exMsg);
                }
            } catch (Exception ex) {
                Console.WriteLine($"ERROR: Unable to read from {iniFile}. {ex}.");
                localConfig = new LocalTopConfig();
                return false;
            }

            try {
                LogConfig.ApplySettingsToNLog(localConfig.LogSettings);
            } catch (Exception ex) {
                Console.WriteLine($"ERROR: Error applying config to NLog. {ex}.");
                localConfig = new LocalTopConfig();
                return false;
            }
            
            return true;
        }
    }
}
