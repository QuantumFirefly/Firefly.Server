using Firefly.Server.Core;
using Firefly.Server.Core.Entities;
using Firefly.Server.Core.Networking;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Data;
using System.Net;
using System.Net.Sockets;

namespace Firefly.Server.Worker
{
    
    internal class IRCListener
    {
        private IFireflyConfig _config;
        private IDbConnection _db;
        private ILogger _log;
        private int _port;
        private IPAddress _ip;
        private TcpListener _listener;
        private IServiceProvider _serviceProvider;
        private bool _isRunning;
        private IGlobalState _globalState;
        public IRCListener(IFireflyConfig config, IServiceProvider serviceProvider, IGlobalState globalState, IDbConnection db, ILogger log) {
            _config = config;
            _db = db;
            _log = log;
            _port = _config.Remote.IRC.Port;
            _ip = _config.Remote.IRC.IP ?? IPAddress.Any;
            _globalState = globalState;
       
            _listener = new TcpListener(_ip, _port);
        }

        public async Task StartAsync() {
            _isRunning = true;
            _listener.Start();
            _log.Log(LogLevel.Info, $"IRC Server now listening on {_ip}:{_port}");

            while (_isRunning) {
                try {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    var clientId = Guid.NewGuid();

                    using (var scope = _serviceProvider.CreateScope()) {
                         var newClientConnection = scope.ServiceProvider.GetRequiredService<Func<TcpClient, Guid, IClientConnection>>()(client, clientId);

                    }

                        
                    
                    

                    //_clients.TryAdd(clientId, handler);
                    //_ = handler.ProcessClientAsync();
                } catch (Exception ex) {
                    _log.Log(LogLevel.Info, $"Error accepting IRC connection: {ex.Message}");
                }
            }
        }

        public async Task StopAsync() {
            _isRunning = false;
            _listener.Stop();

            // Disconnect all clients
            //foreach (var client in _clients.Values) {
            //    await client.DisconnectAsync();
            //}

            _log.Log(LogLevel.Info, $"IRC Server now offline.");
        }

        // todo - build this out later.

    }
}
