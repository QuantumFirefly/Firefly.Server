using Firefly.Server.Worker;
using System.Reflection;


namespace Firefly.Server.ConsoleStartup
{
    
    static class Program
    {
        
        public static void Main(string[] args)
        {
            Console.WriteLine($"Firefly Server Console Booting up...");

            Startup.Run();
        }
    }
}