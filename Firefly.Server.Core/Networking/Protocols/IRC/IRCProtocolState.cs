using Firefly.Server.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Networking.Protocols.IRC
{
    internal class IRCProtocolState : ProtocolStateBase
    {

        public IRCProtocolState(IFireflyConfig state) : base(state) {
        }

        internal string? ClaimedNick;
        internal string? ClaimedRealName;
        internal User? ClaimedUser;
    }
}
