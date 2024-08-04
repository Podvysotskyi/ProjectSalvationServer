using Game.Core;
using Game.Network.Events;
using Game.Network.Tcp.Events;

namespace Game.Network.Tcp
{
    public class NetworkService: IService
    {
        private static NetworkService? _instance;
        public static NetworkService Instance => _instance ??= new NetworkService();
        
        public bool IsRunning { get; private set; }
        public event EventHandler<ConnectionAcceptedEvent>? ConnectionAcceptedEvent;
        public event EventHandler<NetworkReadyEvent>? NetworkReadyEvent;

        private readonly Socket _socket;
        private readonly Dictionary<string, Connection> _connections;

        private NetworkService()
        {
            _socket = new Socket("localhost", 5555);
            _connections = new Dictionary<string, Connection>();
        }
        
        public void Init()
        {
            Console.WriteLine("TCP network: init");
            
            _socket.Init();

            ConnectionAcceptedEvent += OnConnectionAccepted;
            NetworkReadyEvent += OnNetworkReady;
        }

        public void Start()
        {
            if (IsRunning)
            {
                return;
            }
            
            Console.WriteLine("TCP network: start");
            _socket.Start();
            NetworkReadyEvent?.Invoke(this, new NetworkReadyEvent());
        }
        
        public void Stop()
        {
            if (!IsRunning)
            {
                return;
            }
            
            Console.WriteLine("TCP network: stop");
            foreach (var connection in _connections.Values)
            {
                connection.Dispose();
            }
            _connections.Clear();
            _socket.Stop();
        }

        public void Receive()
        {
            foreach (var connection in _connections.Values)
            {
                connection.Receive();
            }
        }

        public void AcceptConnections()
        {
            var connection = _socket.AcceptConnection();

            if (connection == null)
            {
                return;
            }
            
            _connections.Add(connection.Id, connection);
            ConnectionAcceptedEvent?.Invoke(this, new ConnectionAcceptedEvent(connection));
        }
        
        private static void OnConnectionAccepted(object? sender, ConnectionAcceptedEvent e)
        {
            Console.WriteLine("TCP network: connection accepted");
        }

        protected virtual void OnNetworkReady(object? target, NetworkReadyEvent? e)
        {
            Console.WriteLine("TCP network: ready");
        }
    }
}