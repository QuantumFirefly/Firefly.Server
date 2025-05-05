using Firefly.Server.Core.Database;
using Firefly.Server.Core.Entities;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core
{
    public class FireflyContext(IFireflyConfig config, IGlobalState globalState, ILogger logger, IDbConnection dbConnection) : IFireflyContext
    {
        public IFireflyConfig Config { get; } = config;
        public IGlobalState GlobalState { get; } = globalState;
        public ILogger Logger { get; } = logger;
        public IDbConnection DbConnection { get; } = dbConnection;
    }
}
