﻿using Firefly.Server.Core;
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
using IDbConnection = Firefly.Server.Core.Database.IDbConnection;
using Microsoft.Extensions.DependencyInjection;
using Firefly.Server.Core.Networking;
using System.Net.Sockets;
using Firefly.Server.Core.Networking.Protocols;
using Firefly.Server.Core.Networking.Protocols.IRC;


namespace Firefly.Server.Worker
{


    internal class MasterWorker(ILogger? logService = null)
    {

        private readonly ILogger _log = logService ?? LogManager.GetCurrentClassLogger();

        public async Task StartAsync() {
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
                migrator.RunMigrations();

                using var initialDbConnection = new DbConnection(localConfig.DbConnectionSettings, _log);

                _log.Log(LogLevel.Info, $"Loading Firefly Remote Config from Database...");
                if (!ImportandValidateRemoteSettings(out FireflyConfig fireflyConfig, localConfig, initialDbConnection)) {
                    return;
                }

                _log.Log(LogLevel.Debug, $"Building DI Service Collection...");
                var serviceProvider = DependencyInjection.Initalise(fireflyConfig, _log);

                if (fireflyConfig.Remote.IRC.Enabled) {
                    var ircListener = serviceProvider.GetRequiredService<IRCListener>();
                    await ircListener.StartAsync();
                }


            } catch (Exception ex) {
                _log.Log(LogLevel.Fatal, $"{ex.Message}");
                return;
            }
            
            /* TODO   
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
