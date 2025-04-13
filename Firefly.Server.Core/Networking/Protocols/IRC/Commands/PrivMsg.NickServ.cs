using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Networking.Protocols.IRC.Commands
{
    internal partial class PrivMsg // NickServ
    {
        private async Task NickServ(string msg) {
            string firstW = msg.Split(' ', 2)[0];
            // TODO - Check if this is Register or Identify

        }
    }
}
