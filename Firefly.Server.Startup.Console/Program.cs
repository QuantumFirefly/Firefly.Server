using Firefly.Server.Worker;
using System.Reflection;


namespace Firefly.Server.ConsoleStartup
{
    
    static class Program
    {
        
        public static void Main(string[] args) {
            // This is a seperate file because we will have other ways to launch in future. Such as Docker or Windows service.
            Startup.Run("Console");

            // Ensure any errors are visible before closing the console window.
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }
}