﻿using Firefly.Server.Core.Database;
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
    internal partial class PrivMsg : IRCCommandBase
    {
        internal PrivMsg(IFireflyConfig config, IGlobalState globalState, IDbConnection db, ILogger log, IUserRepo userRepo, IRCProtocolState state, Func<string, Task> fnSendMessage)
            : base(config, globalState, db, log, userRepo, state, fnSendMessage) {
            _mustBeAuthenticated = false;
        }

        protected override async Task ExecuteCommand(string args) {
            string[] tempSplitter = args.Split(" ", 2);
            string recipient = tempSplitter[0];
            string restOfText = tempSplitter[1];

            if (recipient.ToUpper() == "NICKSERV") {
                await NickServ(restOfText);
                return;
            }

            if(!_state.IsAuthenticated) {
                await RequestNickServMessage();
                return;
            }
            
        }
    }
}
