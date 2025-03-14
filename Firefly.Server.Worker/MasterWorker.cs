using Firefly.Server.Core;
using Firefly.Server.Core.Database;
using Firefly.Server.Core.Database.Repositories;
using NLog;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Firefly.Server.Core.Entities.RemoteConfig;
using Firefly.Server.Core.Entities.LocalConfig;
using Firefly.Server.Core.Entities;
using DbConnection = Firefly.Server.Core.Database.DbConnection;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace Firefly.Server.Worker
{


    internal class MasterWorker(ILogger? logService = null)
    {

        private readonly ILogger _log = logService ?? LogManager.GetCurrentClassLogger();

        public void Start() {
            try {
                var version = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

                Console.WriteLine($"Firefly Server v{version} Booting up...");
                Console.WriteLine($"Reading {Constants.LOCAL_SETTINGS_INI_FILE}...");

                if (!ImportValidateAndApplyLocalSettings(out LocalTopConfig localConfig, version, Constants.LOCAL_SETTINGS_INI_FILE, Constants.DB_ENVIRONMENT_TYPE)) {
                    return;
                }

                _log.Log(LogLevel.Info, $"Connecting to {localConfig.DbConnectionSettings?.DBMS} Database {localConfig.DbConnectionSettings?.DatabaseName} {localConfig.DbConnectionSettings?.Host}:{localConfig.DbConnectionSettings?.Port}...");
                _log.Log(LogLevel.Info, $"Running migrations...");
                var migrator = new Migrations(localConfig.DbConnectionSettings.ToConnectionString, _log);
                migrator.CreateDbIfNotExists();
                migrator.RunMigrations();

                using var initialDbConnection = new DbConnection(localConfig.DbConnectionSettings, _log);

                _log.Log(LogLevel.Info, $"Loading Firefly Remote Config from Database...");
                if (!ImportandValidateRemoteSettings(out FireflyConfig fireflyConfig, localConfig, initialDbConnection)) {
                    return;
                }

                _log.Log(LogLevel.Debug, $"Building DI Service Collection...");
                var serviceProvider = new ServiceCollection()
                    .AddSingleton<IFireflyConfig>(fireflyConfig)
                    .AddSingleton<ILogger>(_log)
                    .AddScoped<IDbConnection>(p => {
                        var config = p.GetRequiredService<IFireflyConfig>();
                        var log = p.GetRequiredService<ILogger>();
                        return new DbConnection(config.Local?.DbConnectionSettings, log);
                    })
                    .BuildServiceProvider();


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

        private bool ImportandValidateRemoteSettings(out FireflyConfig fireflyConfig, LocalTopConfig localConfig, DbConnection db) {
            fireflyConfig = new FireflyConfig {
                Local = localConfig,
                Remote = new ConfigRepo(db).GetAll().Result
            };

            _log.Log(LogLevel.Debug, $"Validating Firefly Remote Config...");
            List<string> messages = [];
            if (!fireflyConfig.Remote.Validate(ref messages)) {
                var exMsg = string.Join("; ", messages);
                throw new Exception(exMsg);
            }

            return true;
        }

        private bool ImportValidateAndApplyLocalSettings(out LocalTopConfig localConfig, string version, string iniFile, string dbEnvironmentType) {
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

            _log?.Log(LogLevel.Info, $"Firefly Server v{version} - Local Settings Imported & Validated.");
            return true;
        }
    }
}
