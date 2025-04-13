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
            string[] tempSplitter = msg.Split(' ', 2);
            string firstW = tempSplitter[0];
            string restOfText = tempSplitter[1];
            
            if(firstW.ToUpper() == "IDENTIFY") {
                NickServIdentify(restOfText);
            } else if(firstW.ToUpper() == "REGISTER") {
                NickServRegister(restOfText);
            } else {
                // unknown command.
            }

        }

        // TODO - Need to create DB Structure, as well as Login/Register base methods that can be used across all protocols.
        private void NickServIdentify(string msg) {
            
        }

        private void NickServRegister(string msg) {

        }
    }
}
