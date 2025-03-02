using Firefly.Server.Core;
using Firefly.Server.Core.Entitys;
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

        public async Task Start()
        {
            var version = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;

            Console.WriteLine($"Firefly Server v{version} Booting up...");

            Console.WriteLine($"Reading {Constants.LOCAL_SETTINGS_INI_FILE}...");
            LocalSettings localSettings;
            try
            {
                localSettings = new LocalSettingsFactory(Constants.LOCAL_SETTINGS_INI_FILE, Constants.ENVIRONMENT_TYPE).Build();

                var messages = new List<String>();
                if(!localSettings.Validate(ref messages))
                {
                    var exMsg = string.Join("; ", messages);
                    throw new Exception(exMsg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Unable to read from {Constants.LOCAL_SETTINGS_INI_FILE}. {ex.Message}.");
                return;
            }

            localSettings.LogSettings.ApplySettingsToNLog();


            while(true)
            {
                _log.Log(LogLevel.Info, "blah blah");
            }
                
            
    }

}
}
