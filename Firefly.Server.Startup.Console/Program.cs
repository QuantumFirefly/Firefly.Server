using Firefly.Server.Worker;
using System.Reflection;


namespace Firefly.Server.ConsoleStartup
{
    
    static class Program
    {
        
        public static void Main(string[] args) {
            // This is a seperate file because we will have other ways to launch in future. Such as Docker or Windows service.
            Console.WriteLine($"Starting Firefly Server in Console Mode...");

            Startup.Run();

            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }
}