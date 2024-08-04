using System.Net.Sockets;
using Game.Network.Domain.Packages;
using Game.Network.Events;
using Game.Network.Tcp.Threads;

namespace Game.Network.Tcp
{
    public class Connection : IDisposable
    {
        public readonly string Id;
        
        public bool IsOpen { get; private set; }
        
        private readonly System.Net.Sockets.Socket _socket;
        private readonly SendPackagesThread _sendPackagesThread;
        private readonly ReceivePackagesThread _receivePackagesThread;
        
        public event EventHandler<NetworkPackageReceivedEvent>? NetworkPackageReceivedEvent;
        public event EventHandler<NetworkPackageSentEvent>? NetworkPackageSentEvent;

        public Connection(System.Net.Sockets.Socket socket)
        {
            _socket = socket;
            
            IsOpen = true;
            Id = Guid.NewGuid().ToString();
            
            _sendPackagesThread = new SendPackagesThread(socket);
            NetworkPackageSentEvent += OnNetworkPackageSent;
            _sendPackagesThread.Start();

            _receivePackagesThread = new ReceivePackagesThread(socket);
            NetworkPackageReceivedEvent += OnNetworkPackageReceived;
            _receivePackagesThread.Start();
        }
        
        public void Dispose()
        {
            _sendPackagesThread.Stop();
            _receivePackagesThread.Stop();
            
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public void Send(Package package)
        {
            _sendPackagesThread.Send(package);
            NetworkPackageSentEvent?.Invoke(this, new NetworkPackageSentEvent(package));
        }

        public void Receive()
        {
            var packages = _receivePackagesThread.Receive();
            foreach (var package in packages)
            {
                NetworkPackageReceivedEvent?.Invoke(this, new NetworkPackageReceivedEvent(package));
            }
        }

        private void OnNetworkPackageReceived(object? target, NetworkPackageReceivedEvent e)
        {
            Console.WriteLine($"{Id} | TCP network: received package '${e.Type}'");
        }
        
        private void OnNetworkPackageSent(object? target, NetworkPackageSentEvent e)
        {
            Console.WriteLine($"{Id} | TCP network: send package '${e.Type}'");
        }
    }
}