using Firefly.Server.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Networking.Protocols
{
    public class ProtocolStateBase
    {
        private readonly IFireflyConfig _config;
        public string Domain => _config?.Remote?.General?.ServerDomain ?? "";
        public bool IsAuthenticated = false;
        public bool IsSSL = false;

        public ProtocolStateBase(IFireflyConfig config) {
            _config = config;
        }
        
    }
}
