using System.Threading;
using System;
using Firefly.Server.Core.DbSettings;
using System.Reflection;


namespace Firefly.Server.ConsoleStartup
{
    static class Program
    {
        public static void Main(string[] args)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Console.WriteLine($"Firefly Server v{version}");

            Console.WriteLine($"Reading firefly.ini...");
            var dbConnectionStr = new DbSettingsFactory("firefly.ini", "Production").Build();
            if (dbConnectionStr == null)
            {
                Console.WriteLine($"ERROR: ffgdfgdfgsd ");
                return;
            } //test123

            /*
             * 
             * 1. Read db config from ini in app directory.
             * 
             * 
             * 
             */
        }
    }
}