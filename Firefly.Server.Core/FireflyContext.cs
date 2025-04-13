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
    public class FireflyContext : IFireflyContext
    {
        public IFireflyConfig Config { get; }
        public IGlobalState GlobalState { get; }
        public ILogger Logger { get; }

        public FireflyContext(
        IFireflyConfig config,
        IGlobalState globalState,
        ILogger logger) {
            Config = config;
            GlobalState = globalState;
            Logger = logger;
        }

        
    }
}
