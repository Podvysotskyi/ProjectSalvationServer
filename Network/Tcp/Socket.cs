using System.Net;
using System.Net.Sockets;
using Game.Core;
using Game.Network.Tcp.Threads;

namespace Game.Network.Tcp
{
    public class Socket(string host, int port) : IService, IDisposable
    {
        private const int ConnectionLimit = 10;

        public bool IsRunning { get; private set; }

        private System.Net.Sockets.Socket? _socket;
        private AcceptConnectionThread? _acceptConnectionThread;

        ~Socket()
        {
            Dispose();
        }

        public void Dispose()
        {
            Stop();
        }

        public void Init()
        {
            if (IsRunning)
            {
                throw new Exception(); //TODO: Add exception
            }

            var host1 = Dns.GetHostEntry(host);
            var ipAddress = host1.AddressList[0];
            var endpoint = new IPEndPoint(ipAddress, port);

            _socket = new System.Net.Sockets.Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(endpoint);

            _acceptConnectionThread = new AcceptConnectionThread(_socket);
        }

        public void Start()
        {
            if (_socket == null)
            {
                throw new Exception();
            }

            if (IsRunning)
            {
                return;
            }

            _socket.Listen(ConnectionLimit);
            _acceptConnectionThread?.Start();
        }

        public void Stop()
        {
            if (!IsRunning)
            {
                return;
            }

            _acceptConnectionThread?.Stop();

            _socket?.Shutdown(SocketShutdown.Both);
            _socket?.Close();

            IsRunning = false;
        }

        public Connection? AcceptConnection()
        {
            var socket = _acceptConnectionThread?.Accept();

            return socket == null ? null : new Connection(socket);
        }
    }
}