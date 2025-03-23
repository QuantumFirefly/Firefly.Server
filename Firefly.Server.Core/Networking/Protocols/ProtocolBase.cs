using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Networking.Protocols
{
    public abstract class ProtocolBase : IProtocol
    {
        private readonly string _protocolName;
        protected ProtocolBase(string ProtocolName) {
            _protocolName = ProtocolName;
        }

        public string GetProtocolName() { return _protocolName; }

        public abstract void Parse(string input);
    }
}
