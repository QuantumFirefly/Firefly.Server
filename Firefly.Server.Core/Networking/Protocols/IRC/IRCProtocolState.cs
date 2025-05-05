using Firefly.Server.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Networking.Protocols.IRC
{
    public class IRCProtocolState(IFireflyConfig state) : ProtocolStateBase(state)
    {

        public string? ClaimedNick;
        public string? ClaimedRealName;
        public User? ClaimedUser;
    }
}
