using Firefly.Server.Core.Networking;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core
{
    /*
     * 
     * For now we'll use LINQ to access objects, to speed up development, however this may want to be adjusted to use 
     * an EF Core In-memory DB once we hit 50k+ concurrent users connected to Firefly servers.
     * 
     * Index can be created in order to speed up LINQ queries to help us get up to about 50k users before a migration is required.
     * Performance testing will be needed to determine the best course of action.
     * EF-Core in-memory can be used so that LINQ queries can still be used.
     * 
     */
    public class GlobalState : IGlobalState
    {
        private readonly ConcurrentDictionary<Guid, IClientConnection> _connectedClients = new ();

        public ConcurrentDictionary<Guid, IClientConnection> GetConnectedClients() => _connectedClients;
                                                                                                         
    }
}
