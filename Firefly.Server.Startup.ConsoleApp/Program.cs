using Firefly.Server.Worker;
using System.Reflection;


namespace Firefly.Server.Startup.ConsoleApp
{
    
    static class Program
    {
        
        public async static Task Main() {
            // This is a separate file because we will have other ways to launch in future. Such as Docker or Windows service.
            await Worker.Startup.Run("Console");

            // Ensure any errors are visible before closing the console window.
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }
}