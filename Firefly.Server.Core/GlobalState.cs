using Firefly.Server.Core.Networking;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core
{
    public class GlobalState : IGlobalState
    {
        private readonly ConcurrentDictionary<Guid, IClientConnection> _connectedClients = new ();

        public ConcurrentDictionary<Guid, IClientConnection> GetConnectedClients() => _connectedClients; // TODO - This entire class needs to become an in-memory DB.
                                                                                                         // Linq is not quick enough
    }
}
