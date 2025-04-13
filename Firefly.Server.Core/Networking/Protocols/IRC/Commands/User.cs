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
    internal class User : IRCCommandBase
    {
        internal User(IFireflyConfig config, IGlobalState globalState, IDbConnection db, ILogger log, IUserRepo userRepo, IRCProtocolState state, Func<string, Task> fnSendMessage)
            : base(config, globalState, db, log, userRepo, state, fnSendMessage) {

        }

        protected override async Task ExecuteCommand(string args) {
            _state.ClaimedRealName = args.Split(":", 2)[1];
            string username = args.Split(" ", 2)[0];

            // For IRC, we want to record the user they are claiming to be, and then force a login/register via NickServ later.
            _state.ClaimedUser = await _userRepo?.GetByUsername(username);

            await recievedNickAndUser();
        }
    }
}
