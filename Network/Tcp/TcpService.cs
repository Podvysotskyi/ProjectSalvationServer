using Game.Core;
using Game.Network.Tcp.Events;

namespace Game.Network.Tcp;

public class TcpService : IService
{
    public bool IsRunning { get; private set; }
    public Event<TcpConnectionEvent> ConnectionAcceptedEvent = new();
    public event EventHandler<NetworkReadyEvent>? NetworkReadyEvent;

    private readonly TcpSocket _tcpSocket;
    private readonly Dictionary<string, TcpConnection> _connections;

    public TcpService(string host, int port)
    {
        _tcpSocket = new TcpSocket(host, port);
        _connections = new Dictionary<string, TcpConnection>();
    }

    public void Init()
    {
        Console.WriteLine("TCP network: init");

        _tcpSocket.Init();

        ConnectionAcceptedEvent.AddListener(OnConnectionAccepted);
        NetworkReadyEvent += OnNetworkReady;
    }

    public void Start()
    {
        if (IsRunning)
        {
            return;
        }

        Console.WriteLine("TCP network: start");
        _tcpSocket.Start();

        IsRunning = true;
        
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
        _tcpSocket.Stop();

        IsRunning = false;
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
        var connection = _tcpSocket.AcceptConnection();

        if (connection == null)
        {
            return;
        }

        _connections.Add(connection.Id, connection);
        ConnectionAcceptedEvent.Invoke(new TcpConnectionEvent(connection));
    }

    private static void OnConnectionAccepted(TcpConnectionEvent e)
    {
        Console.WriteLine("TCP network: connection accepted");
    }

    protected virtual void OnNetworkReady(object? target, NetworkReadyEvent? e)
    {
        Console.WriteLine("TCP network: ready");
    }
}