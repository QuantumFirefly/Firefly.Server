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
        protected readonly IFireflyContext _context;
        protected readonly IDbContext _dbContext;

        protected readonly ILogger _log;

        protected Func<string, Task> _fnSendMessage;

        protected ProtocolBase(string ProtocolName, IFireflyContext context, IDbContext dbContext) {
            _protocolName = ProtocolName;
            _context = context;
            _dbContext = dbContext;

            _log = context.Logger;
        }

        public void SetFnSendMessage(Func<string, Task> fnSendMessage) {
            _fnSendMessage = fnSendMessage;
        } 

        public string GetProtocolName() { return _protocolName; }

        public abstract Task Parse(string input);

        protected async Task SendMessageAsync(string message) {
            if (_fnSendMessage != null) {
                _log.Log(LogLevel.Trace, $"[{_protocolName}] Sending: {message}");
                await _fnSendMessage?.Invoke(message);
            }
        }
    }
}
