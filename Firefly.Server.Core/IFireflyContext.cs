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
    public interface IFireflyContext
    {
        IFireflyConfig Config { get; }
        IGlobalState GlobalState { get; }
        ILogger Logger { get; }
        IDbConnection DbConnection { get; }
    }
}
