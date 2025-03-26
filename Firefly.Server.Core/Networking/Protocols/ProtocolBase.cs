using Firefly.Server.Core.Database;
using Firefly.Server.Core.Database.Repositories;
using Firefly.Server.Core.Entities;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Networking.Protocols
{
    public abstract class ProtocolBase : IProtocol
    {
        private readonly string _protocolName;
        protected readonly IFireflyConfig _config;
        protected readonly ILogger _log;
        protected readonly IGlobalState _globalState;
        protected readonly IDbConnection _db;
        protected readonly IUserRepo _userRepo;

        protected Func<string, Task> _fnSendMessage;

        protected ProtocolBase(string ProtocolName, IFireflyConfig config, IGlobalState globalState, IDbConnection db, ILogger log, IUserRepo userRepo) {
            _protocolName = ProtocolName;
            _config = config;
            _globalState = globalState;
            _db = db;
            _log = log;
            _userRepo = userRepo;
        }

        public void SetFnSendMessage(Func<string, Task> fnSendMessage) {
            _fnSendMessage = fnSendMessage;
        } 

        public string GetProtocolName() { return _protocolName; }

        public abstract Task Parse(string input);

        protected async Task SendMessage(string message) {
            if (_fnSendMessage != null) {
                await _fnSendMessage?.Invoke(message);
            }
        }
    }
}
