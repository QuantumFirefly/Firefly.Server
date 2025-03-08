using DbUp;
using DbUp.Engine.Output;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using NLog;
using Npgsql.Internal;
using System.Reflection;
using ILogger = NLog.ILogger;

namespace Firefly.Server.Core.Database
{
    public class Migrations(string connectionString, ILogger log)
    {
        public void CreateDbIfNotExists() {
            // EnsureDatabase writes to Console and does allow custom ILoggers.
            // Need to intercept the console output.
            var consoleInterceptor = new StringWriter();
            try {
                Console.SetOut(consoleInterceptor);

                EnsureDatabase.For.PostgresqlDatabase(connectionString);
            } finally {
                // Reset the console to normal cmd output!
                string debugLog = consoleInterceptor.ToString();
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

                if (debugLog.Length > 0) log.Debug(debugLog);
            }
        }

        public void RunMigrations() {
            _ = DeployChanges
                .To.
                PostgresqlDatabase(connectionString)
                .LogTo(new DbUpToNLog(log))
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(),
                    filter => filter.StartsWith("Firefly.Server.Core.Database.MigrationSQL") 
                    && filter.EndsWith(".psql"))
                .Build()
                .PerformUpgrade();
        }
    }
}
