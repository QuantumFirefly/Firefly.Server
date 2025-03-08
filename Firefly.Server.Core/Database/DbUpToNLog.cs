using DbUp.Engine.Output;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Database
{
    public class DbUpToNLog(ILogger log) : IUpgradeLog
    {
        public void LogTrace(string format, params object[] args) => log.Trace(format, args);

        public void LogDebug(string format, params object[] args) => log.Debug(format, args);

        // Suppress DbUp output
        public void LogInformation(string format, params object[] args) => log.Debug(format, args); 

        public void LogWarning(string format, params object[] args) => log.Warn(format, args);

        public void LogError(string format, params object[] args) => log.Error(format);

        public void LogError(Exception ex, string format, params object[] args) => throw ex;
    }
}
