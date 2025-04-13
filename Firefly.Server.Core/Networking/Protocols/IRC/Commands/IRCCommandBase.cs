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
    internal abstract class IRCCommandBase
    {
        protected readonly IFireflyConfig _config;
        protected readonly IGlobalState _globalState;
        protected readonly IDbConnection _db;
        protected readonly ILogger _log;
        protected readonly IUserRepo _userRepo;

        protected IRCProtocolState _state;
        protected bool _mustBeAuthenticated;

        protected Func<string, Task> _fnSendMessage;

        internal IRCCommandBase(IFireflyConfig config, IGlobalState globalState, IDbConnection db, ILogger log, IUserRepo userRepo, IRCProtocolState state, Func<string, Task> fnSendMessage) {
            _config = config;
            _globalState = globalState;
            _db = db;
            _log = log;
            _userRepo = userRepo;

            _state = state;
            _fnSendMessage = fnSendMessage;
        }
        internal async Task Execute(string args) {
            if (_mustBeAuthenticated) {
                await RequestNickServMessage();
                return;
            }

            await ExecuteCommand(args);
        }

        protected abstract Task ExecuteCommand(string args);

        protected async Task SendMessageAsync(string message) {
            _fnSendMessage(message);
        }

        protected async Task recievedNickAndUser() {
            if (string.IsNullOrEmpty(_state.ClaimedRealName) || string.IsNullOrEmpty(_state.ClaimedNick)) {
                return;
            }

            await SendMOTD();
            await RequestNickServMessage();
        }

        protected async Task RequestNickServMessage() {

            string message = _state.ClaimedUser != null
                ? "You have not identified with NickServ. Please identify."
                : "You have not registered with NickServ. Please register and identify.";

            if (!_state.IsSSL) {
                message += " You should be aware that this is an unsecure connection and your password will be transmitted in plain text.";
            }

            await SendMessageAsync($"451 {_state.ClaimedUser} :{message}");
        }

        private async Task SendMOTD() {
            string? motd = _config?.Remote?.IRC?.MOTD;

            if (motd != null) {
                string[] motdLines = motd.Split("\n");

                for (int i = 0; i < motdLines.Length; i++) {
                    string code = i < motdLines.Length - 1 ? "372" : "376"; // Last line is 376, all others are 372.
                    await SendMessageAsync($"{code} {_state.ClaimedNick} :{motdLines[i]}");
                }

            } else {
                await SendMessageAsync($"422 {_state.ClaimedNick} :No MOTD. Welcome to {_state.Domain}, a Firefly Server.");
            }
        }

        
    }
}
