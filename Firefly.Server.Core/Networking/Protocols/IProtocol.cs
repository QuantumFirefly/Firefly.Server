using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Networking.Protocols
{
    public interface IProtocol
    {
        public string GetProtocolName();
        public Task Parse(string input);
        public void SetFnSendMessage(Func<string, Task> fnSendMessage);
    }
}
