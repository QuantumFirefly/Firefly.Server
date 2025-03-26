using Firefly.Server.Core.Database;
using Firefly.Server.Core.Database.Repositories;
using Firefly.Server.Core.Entities;
using NLog;
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
        public IRCProtocol(IFireflyConfig config, IGlobalState globalState, IDbConnection db, ILogger log, IUserRepo userRepo)
            : base("IRC", config, globalState, db, log, userRepo)  {
            
        }

        private string _claimedNick;
        private string _claimedRealName;
        private User? _claimedUser;

        public override async Task Parse(string message) { // TODO - Must consider multiple messages & partial messages. IRC is split by '\r\n'
            string[] firstAndRemaining = message.Split(" ", 2);
            string firstWord = firstAndRemaining[0];
            string restOfText = firstAndRemaining[1];

            switch(firstWord) {
                case "USER":
                    await parseUSER(restOfText);
                    break;
                case "NICK":
                    await parseNICK(restOfText);
                    break;
            }
        }

#region IRC Parse Methods
        private async Task parseUSER(string restOfText) {
            _claimedRealName = restOfText.Split(":", 2)[1];
            string username = restOfText.Split(" ", 2)[0];

            // For IRC, we want to record the user they are claiming to be, and then force a login/register via NickServ later.
            _claimedUser = await _userRepo?.GetByUsername(username);

            await recievedNickAndUser();
        }

        private async Task parseNICK(string RestOfText) {
            _claimedNick = RestOfText;

            await recievedNickAndUser();
        }

        private async Task recievedNickAndUser() {
            if (String.IsNullOrEmpty(_claimedRealName) || String.IsNullOrEmpty(_claimedNick)) {
                return;
            }

            //_fnSendMessage?.Invoke(_config.Remote.IRC.MOTD); 

            // TODO - Send successfully connected messages here. That's one code if MOTD exists or another if null.
            await SendMessage("422"); // Here as a test

            // TODO - Send message about requireing NickServ register/identify after MOTD.
        }
        #endregion

    }
}
