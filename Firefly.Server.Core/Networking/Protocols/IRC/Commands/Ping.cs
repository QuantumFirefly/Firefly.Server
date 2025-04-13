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
    internal class Ping : IRCCommandBase
    {
        internal Ping(IFireflyConfig config, IGlobalState globalState, IDbConnection db, ILogger log, IUserRepo userRepo, IRCProtocolState state, Func<string, Task> fnSendMessage)
            : base(config, globalState, db, log, userRepo, state, fnSendMessage) {

        }

        protected override async Task ExecuteCommand(string args) {

            await SendMessageAsync($"PONG {_state.Domain} {args}");
        }
    }
}
