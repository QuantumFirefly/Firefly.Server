﻿using Firefly.Server.Worker;
using System.Reflection;


namespace Firefly.Server.ConsoleStartup
{
    
    static class Program
    {
        
        public static void Main(string[] args)
        {
            // This is a seperate file because we will have other ways to launch in future. Such as Docker or Windows service.
            Console.WriteLine($"Firefly Server Console Booting up...");

            Startup.Run();
        }
    }
}