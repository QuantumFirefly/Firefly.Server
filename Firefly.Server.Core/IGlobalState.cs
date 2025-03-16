using Firefly.Server.Core.Networking;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core
{
    public interface IGlobalState
    {
        public ConcurrentDictionary<Guid, IClientConnection> GetConnectedClients();
    }
}
