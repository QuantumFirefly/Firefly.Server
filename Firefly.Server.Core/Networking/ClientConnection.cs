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
        public ClientConnection(TcpClient client, Guid clientId, IGlobalState globalState, ILogger log, IProtocol protocol) {
            _tcpconnection = client;
            Id = clientId;
            IP = IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
            Port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
            _stream = _tcpconnection.GetStream();
            _log = log;
            _protocol = protocol;
            _protocol.SetFnSendMessage(SendMessageAsync);

            _log.Log(LogLevel.Info, $"[{protocol.GetProtocolName()}] New connection from {IP}");
            _ = ProcessStream();
        }

        private async Task ProcessStream() {
            try {
                byte[] buffer = new byte[Constants.IRC_NETWORK_BUFFER_SIZE];
                while (_tcpconnection.Connected) {
                    
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) {
                        _log.Log(LogLevel.Info, "Client Disconnected.");
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    _log.Log(LogLevel.Trace, $"[{IP} : {message}");
                    await _protocol.Parse(message);
                    // TODO - May contain multiple or 'partial' messages. IRC typically uses \r\n as a terminator.


                }
            } catch (Exception ex) {
                _log.Log(LogLevel.Error, ex.ToString());
            } finally {
                await DisconnectAsync();
            }
        }

        public async Task DisconnectAsync() {
            _tcpconnection.Close(); // TODO - Need to remove "dead" client Connection from Global State.
        }

        internal async Task SendMessageAsync(string message) {
            if (_tcpconnection.Connected) {
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                await _stream.WriteAsync(messageBytes, 0, messageBytes.Length);
            }
        }


    }
}
