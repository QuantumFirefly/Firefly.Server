using Firefly.Server.Core.Database;
using Firefly.Server.Core.Database.Repositories;
using Firefly.Server.Core.Entities;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Networking.Protocols.IRC.Commands
{
    public class Cap : IRCCommandBase
    {
        public Cap(IFireflyContext context, IDbContext dbContext, IRCProtocolState state, Func<string, Task> fnSendMessage)
            : base(context, dbContext, state, fnSendMessage) {

        }

        protected override async Task ExecuteCommand(string args) {
            // TODO - Do nothing for now.
        }
    }
}
