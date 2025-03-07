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


namespace Firefly.Server.Worker
{


    internal class MasterWorker(ILogger? logService = null)
    {

        private readonly ILogger _log = logService ?? LogManager.GetCurrentClassLogger();

        public void Start() {
            var version = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;

            Console.WriteLine($"Firefly Server v{version} Booting up...");

            Console.WriteLine($"Reading {Constants.LOCAL_SETTINGS_INI_FILE}...");

            if (!ImportValidateAndApplyLocalSettings(out LocalConfig localConfig, Constants.LOCAL_SETTINGS_INI_FILE, Constants.DB_ENVIRONMENT_TYPE)) {
                return;
            }

            _log.Log(LogLevel.Info, $"Firefly Server v{version} - Local Settings Imported & Validated.");

            _log.Log(LogLevel.Info, $"Connecting to {localConfig.DbConnectionSettings.DBMS} Database {localConfig.DbConnectionSettings.Host}:{localConfig.DbConnectionSettings.Port}...");
            using var dbConnection = new DbConnection(localConfig.DbConnectionSettings);
            try {
                dbConnection.Open();

                _log.Log(LogLevel.Debug, $"Database Connected!");


                _log.Log(LogLevel.Info, $"Loading Firefly Config from Database...");
                RemoteConfig fireflyConfig = new ConfigRepo(dbConnection).GetAll().Result;

                _log.Log(LogLevel.Debug, $"Validating Firefly Config...");
                List<string> messages = [];
                if (!fireflyConfig.Validate(ref messages)) {
                    var exMsg = string.Join("; ", messages);
                    throw new Exception(exMsg);
                }


            } catch (Exception ex) {
                _log.Log(LogLevel.Fatal, $"{ex.Message}");
                return;
            }
                
           

            

            // TODO - Create tool that takes db info from Local Settings. Takes super user password, and creates DB & user/pass
            
            /* TODO
             * Create FireflyConfig.cs that holds both RemoteConfig & LocalConfig classes.
             * Fix new Warnings & Messages
             * DbUp installation - research for alternatives?

            
             * Start to listen in on IRC port for inbound TCP connections.
             * Classes need abstracting to Interfaces for DI & Unit Testing.
             * 
             * Integration & Unit Tests
             */


        }

        private static bool ImportValidateAndApplyLocalSettings(out LocalConfig localConfig, string iniFile, string dbEnvironmentType) {
            try {
                localConfig = LocalConfig.Build(iniFile, dbEnvironmentType);

                var messages = new List<String>();
                if (!localConfig.Validate(ref messages)) {
                    var exMsg = string.Join("; ", messages);
                    throw new Exception(exMsg);
                }
            } catch (Exception ex) {
                Console.WriteLine($"ERROR: Unable to read from {iniFile}. {ex}.");
                localConfig = new LocalConfig();
                return false;
            }

            try {
                LogConfig.ApplySettingsToNLog(localConfig.LogSettings);
            } catch (Exception ex) {
                Console.WriteLine($"ERROR: Error applying config to NLog. {ex}.");
                localConfig = new LocalConfig();
                return false;
            }
            
            return true;
        }
    }
}
