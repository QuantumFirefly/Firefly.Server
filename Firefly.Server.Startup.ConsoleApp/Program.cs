using Firefly.Server.Worker;


namespace Firefly.Server.Startup.ConsoleApp
{
    
    static class Program
    {
        
        public static void Main() {
            // This is a seperate file because we will have other ways to launch in future. Such as Docker or Windows service.
            Worker.Startup.Run("Console");

            // Ensure any errors are visible before closing the console window.
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }
}