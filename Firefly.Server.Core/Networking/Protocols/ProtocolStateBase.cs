using Firefly.Server.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Networking.Protocols
{
    internal class ProtocolStateBase
    {
        private readonly IFireflyConfig _config;
        internal string Domain => _config?.Remote?.General?.ServerDomain ?? "";
        internal bool IsAuthenticated = false;
        internal bool IsSSL = false;

        internal ProtocolStateBase(IFireflyConfig config) {
            _config = config;
        }
        
    }
}
