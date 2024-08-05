using Game.Core;
using Game.Network.Tcp.Events;

namespace Game.Network.Tcp;

public class TcpService : IService
{
    public bool IsRunning { get; private set; }

    private readonly TcpSocket _tcpSocket;
    private readonly Dictionary<string, TcpConnection> _connections;

    public readonly Event<TcpConnectionEvent> ConnectionAcceptedEvent = new();
    
    public TcpService(string host, int port)
    {
        _tcpSocket = new TcpSocket(host, port);
        _connections = new Dictionary<string, TcpConnection>();
    }

    public void Init()
    {
        Console.WriteLine("TCP network: init");

        _tcpSocket.Init();
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
        
        Console.WriteLine("TCP network: ready");
    }

    public void Stop()
    {
        if (!IsRunning)
        {
            return;
        }

        Console.WriteLine("TCP network: stop");
        foreach (var key in _connections.Keys)
        {
            _connections[key].Stop();
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
        Console.WriteLine($"{connection.Id} | TCP network: connection accepted");
        ConnectionAcceptedEvent.Invoke(new TcpConnectionEvent(connection));
    }

    public void CloseConnections()
    {
        foreach (var key in _connections.Keys)
        {
            _connections[key].Update();

            if (!_connections[key].IsOpen)
            {
                _connections.Remove(key);
                break;
            }
        }
    }
}