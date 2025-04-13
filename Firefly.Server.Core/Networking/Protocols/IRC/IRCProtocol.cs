using Firefly.Server.Core.Database;
using Firefly.Server.Core.Database.Repositories;
using Firefly.Server.Core.Entities;
using Firefly.Server.Core.Networking.Protocols.IRC.Commands;
using NLog;
using System.Text;
using User = Firefly.Server.Core.Networking.Protocols.IRC.Commands.User;

namespace Firefly.Server.Core.Networking.Protocols.IRC
{
    public class IRCProtocol : ProtocolBase
    {
        public IRCProtocol(IFireflyConfig config, IGlobalState globalState, IDbConnection db, ILogger log, IUserRepo userRepo)
            : base("IRC", config, globalState, db, log, userRepo)  {
            _state = new IRCProtocolState(_config);
            InitCommands();
        }

        private readonly Dictionary<string, IRCCommandBase> _commands = new(StringComparer.OrdinalIgnoreCase);
        private readonly StringBuilder _receivedTcpStream = new();
        private readonly IRCProtocolState _state;

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

        private async Task SendMessageAsync(string message) {
            await base.SendMessageAsync($":{_state.Domain} {message}\r\n");
        }

        private async Task ParseLine(string message) {
            string[] firstAndRemaining = message.Split(" ", 2);
            string firstWord = firstAndRemaining[0];
            string restOfText = firstAndRemaining[1];

            IRCCommandBase? command = _commands.GetValueOrDefault(firstWord, null);
            if (command == null) {
                await SendMessageAsync($"421 {_state.ClaimedNick} {firstWord} :Unknown command");
                return;
            }

            await command.Execute(restOfText);
        }

        private void InitCommands() { // TODO, can we use DI to load these?
            _commands.Add("NICK", new Nick(_config, _globalState, _db, _log, _userRepo, _state, SendMessageAsync));
            _commands.Add("USER", new User(_config, _globalState, _db, _log, _userRepo, _state, SendMessageAsync));
            _commands.Add("PING", new Ping(_config, _globalState, _db, _log, _userRepo, _state, SendMessageAsync));
            _commands.Add("CAP", new Cap(_config, _globalState, _db, _log, _userRepo, _state, SendMessageAsync));
            _commands.Add("PRIVMSG", new PrivMsg(_config, _globalState, _db, _log, _userRepo, _state, SendMessageAsync));
        }
    }
}
