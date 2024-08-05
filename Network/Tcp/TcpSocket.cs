using System.Net;
using System.Net.Sockets;
using Game.Core;
using Game.Network.Tcp.Workers;

namespace Game.Network.Tcp;

public class TcpSocket: IService, IDisposable
{
    private const int ConnectionLimit = 10;

    private readonly string _host;
    private readonly int _port;

    public bool IsRunning { get; private set; }

    private Socket? _socket;
    private AcceptConnectionWorker? _acceptConnectionThread;

    public TcpSocket(string host, int port)
    {
        _host = host;
        _port = port;
    }
        
    ~TcpSocket()
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

        var host = Dns.GetHostEntry(_host);
        var ipAddress = host.AddressList[0];
        var endpoint = new IPEndPoint(ipAddress, _port);

        _socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _socket.Bind(endpoint);

        _acceptConnectionThread = new AcceptConnectionWorker(_socket);
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
            
        Console.WriteLine("TCP socket: start");

        _socket.Listen(ConnectionLimit);
        _acceptConnectionThread?.Start();

        IsRunning = true;
    }

    public void Stop()
    {
        if (!IsRunning)
        {
            return;
        }
            
        Console.WriteLine("TCP socket: stop");
        _acceptConnectionThread?.Stop();

        _socket?.Shutdown(SocketShutdown.Both);
        _socket?.Close();

        IsRunning = false;
    }

    public TcpConnection? AcceptConnection()
    {
        var socket = _acceptConnectionThread?.Accept();

        return socket == null ? null : new TcpConnection(socket);
    }
}