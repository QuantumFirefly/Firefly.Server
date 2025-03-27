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

namespace Firefly.Server.Core.Networking.Protocols.IRC
{
    public class IRCProtocol : ProtocolBase
    {
        public IRCProtocol(IFireflyConfig config, IGlobalState globalState, IDbConnection db, ILogger log, IUserRepo userRepo)
            : base("IRC", config, globalState, db, log, userRepo)  {
            
        }

        private readonly StringBuilder _receivedTcpStream = new();

        private string _claimedNick;
        private string _claimedRealName;
        private User? _claimedUser;

        #region IRC Initial Message Parseing

        public override async Task Parse(string message) {
            _receivedTcpStream.Append(message);

            string[] messages = GetMessagesFromStream();

            foreach (string curLine in messages) {
                await ParseLine(curLine);
            }
        }

        private string[] GetMessagesFromStream() {
            string fullStream = _receivedTcpStream.ToString();
            string[] messages = fullStream.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            bool endsWithNewline = fullStream.EndsWith("\r\n");

            _receivedTcpStream.Clear();
            if (!endsWithNewline) {
                _receivedTcpStream.Append(messages.Last());
                messages = messages.Take(messages.Length - 1).ToArray();
            }

            return messages;
        }

        private async Task ParseLine(string message) {
            string[] firstAndRemaining = message.Split(" ", 2);
            string firstWord = firstAndRemaining[0];
            string restOfText = firstAndRemaining[1];

            // Limited access to IRC protocol prior to authentication
            switch (firstWord) {
                case "USER":
                    await parseUSER(restOfText);
                    break;
                case "NICK":
                    await parseNICK(restOfText);
                    break;
                case "PING": // TODO - Need to Send Pings as well if no messages in x time.
                    await parsePING(restOfText);
                    break;
                case "PRIVMSG":
                    if (!_isAuthenticated) {
                        string secondWord = restOfText.Split(" ", 2)[0];

                        if (secondWord.ToUpper() == "NICKSERV") {
                            // TODO - Implement NickServ Register/Login here
                        } else {
                            await RequestNickServMessage();
                        }
                    }
                        break;
                default:
                    if(!_isAuthenticated) await RequestNickServMessage();
                    break;
            }

            if (!_isAuthenticated) return;

            // We can assume the user is authenticated here
            switch(firstWord) {
                case "":
                    break;
            }
        }
        #endregion

        #region IRC Commands Methods
        private async Task parseUSER(string restOfText) {
            _claimedRealName = restOfText.Split(":", 2)[1];
            string username = restOfText.Split(" ", 2)[0];

            // For IRC, we want to record the user they are claiming to be, and then force a login/register via NickServ later.
            _claimedUser = await _userRepo?.GetByUsername(username);

            await recievedNickAndUser();
        }

        private async Task parseNICK(string restOfText) {
            _claimedNick = restOfText;
            await recievedNickAndUser();
        }

        private async Task parsePING(string restOfText) {
            await SendMessageAsync($"PONG {_domain} {restOfText}");
        }

        #endregion

        #region Other Methods
        private async Task recievedNickAndUser() {
            if (string.IsNullOrEmpty(_claimedRealName) || string.IsNullOrEmpty(_claimedNick)) {
                return;
            }

            await SendMOTD(); 
            }

        private async Task SendMOTD() {
            string? motd = _config?.Remote?.IRC?.MOTD;
            if (motd != null) {
                string[] motdLines = motd.Split("\n");

                for (int i = 0; i < motdLines.Length; i++) {
                    string code = i < motdLines.Length - 1 ? "372" : "376"; // Last line is 376, all others are 372.
                    await SendMessageAsync($"{code} {_claimedNick} :{motdLines[i]}");
                }

            } else {
                await SendMessageAsync($"422 {_claimedNick} :No MOTD. Welcome to {_domain}, a Firefly Server.");
            }
        }

        private async Task RequestNickServMessage() {

            string message = _claimedUser != null
                ? "You have not identified with NickServ. Please identify."
                : "You have not registered with NickServ. Please register and identify.";

            if (!_isSSL) {
                message += " You should be aware that this is an unsecure connection and your password will be transmitted in plain text.";
            }

            await SendMessageAsync($"451 {_claimedNick} :{message}");
        }
        #endregion

        #region IRC Message Sending

        private async Task SendMessageAsync(string message) {
            await base.SendMessageAsync($":{_domain} {message}\r\n");
        }

        #endregion

    }
}
