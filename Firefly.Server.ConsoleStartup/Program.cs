using Firefly.Server.Core;
using System.Reflection;


namespace Firefly.Server.ConsoleStartup
{
    
    static class Program
    {
        private const string LOCAL_SETTINGS_INI_FILE = "firefly.ini";
        public static void Main(string[] args)
        {
            var version = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;

            Console.WriteLine($"Firefly Server v{version} Booting up...");

            Console.WriteLine($"Reading {LOCAL_SETTINGS_INI_FILE}...");
            try
            {
                var dbConnectionStr = new LocalSettingsFactory(LOCAL_SETTINGS_INI_FILE, "Production").Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Unable to read from {LOCAL_SETTINGS_INI_FILE}. {ex.Message}.");
            }

            // TODO - Move this into Worker.
        }
    }
}