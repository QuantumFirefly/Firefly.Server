using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Firefly.Server.Core.Networking.Protocols
{
    public class IRCProtocol : ProtocolBase
    {
        public IRCProtocol() : base("IRC")  {
            // TODO - Need to use DI to take in ILogger, IState, etc.
        }

        public override void Parse(string message) {
            throw new NotImplementedException();
        }

    }
}
