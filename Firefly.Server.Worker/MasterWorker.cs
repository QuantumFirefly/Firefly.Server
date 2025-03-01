using Firefly.Server.Core;
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

        public MasterWorker() { } // For dependency injection.

        public async Task Start()
        {
            var version = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;

            Console.WriteLine($"Firefly Server v{version} Booting up...");

            Console.WriteLine($"Reading {Constants.LOCAL_SETTINGS_INI_FILE}...");
            try
            {
                var localSettings = new LocalSettingsFactory(Constants.LOCAL_SETTINGS_INI_FILE, "Production").Build();

                string message;
                if(!localSettings.Validate(out message))
                {
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Unable to read from {Constants.LOCAL_SETTINGS_INI_FILE}. {ex.Message}.");
                return;
            }
        }

    }
}
