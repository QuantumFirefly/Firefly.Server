using Firefly.Server.Core.Networking.Protocols;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Firefly.Server.Core.Networking
{
    public class ClientConnection : IClientConnection
    {
        public Guid Id { get; }
        public IPAddress IP { get; }
        public int Port { get; }
        private TcpClient _tcpconnection { get; set; }
        private NetworkStream _stream { get; }
        private IProtocol _protocol { get; set; }
        private ILogger _log { get; set; }
        public ClientConnection(TcpClient client, Guid clientId, IGlobalState globalState, ILogger log) {
            _tcpconnection = client;
            Id = clientId;
            IP = IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
            Port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
            _stream = _tcpconnection.GetStream();
            _log = log;

            _log.Log(LogLevel.Info, $"New connection from {IP}"); // TODO Should take in IProtocol which returns the protocol name?
        }

        
    }
}
