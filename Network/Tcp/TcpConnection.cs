using System.Net.Sockets;
using Game.Core;
using Game.Network.Package;
using Game.Network.Package.Types;
using Game.Network.Tcp.Events;
using Game.Network.Tcp.Workers;

namespace Game.Network.Tcp;

public class TcpConnection : IDisposable
{
    public const int MaxPackageDelay = 10;
    
    public readonly string Id;
    public readonly Event<TcpConnectionEvent> ConnectionClosedEvent = new();
        
    public bool IsOpen { get; private set; }
    public DateTime LastPackageDateTime { get; private set; }
    
    private readonly Socket _socket;
    private readonly SendPackagesWorker _sendPackagesWorker;
    private readonly ReceivePackagesWorker _receivePackagesWorker;
    private readonly Dictionary<NetworkPackageType, Event<TcpPackageEvent>> _networkPackageEvent = new();

    public TcpConnection(Socket socket)
    {
        _socket = socket;
            
        IsOpen = true;
        Id = Guid.NewGuid().ToString();
        LastPackageDateTime = DateTime.Now;
            
        _sendPackagesWorker = new SendPackagesWorker(socket);
        _sendPackagesWorker.Start();

        _receivePackagesWorker = new ReceivePackagesWorker(socket);
        _receivePackagesWorker.Start();
    }

    public void Dispose()
    {
        try
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
        catch
        {
            // ignored
        }
    }

    public void Stop()
    {
        if (!IsOpen)
        {
            return;
        }
            
        _sendPackagesWorker.Stop();
        _receivePackagesWorker.Stop();
            
        Dispose();
        
        Console.WriteLine($"{Id} | TCP network: connection closed");
        ConnectionClosedEvent.Invoke(new TcpConnectionEvent(this));

        IsOpen = false;
    }

    public void Send(NetworkPackage package)
    {
        if (!IsOpen)
        {
            return;
        }
        
        _sendPackagesWorker.Send(package);
        LastPackageDateTime = DateTime.Now;
        
        Console.WriteLine($"{Id} | TCP network: send package '{package.Type}'");
    }

    public void Receive()
    {
        if (!IsOpen)
        {
            return;
        }
        
        var packages = _receivePackagesWorker.Receive();
        
        for (var i = 0; i < packages.Count; i++)
        {
            Console.WriteLine($"{Id} | TCP network: received package '{packages[i].Type}'");
            LastPackageDateTime = DateTime.Now;
            this[packages[i].Type].Invoke(new TcpPackageEvent(this, packages[i]));
        }

        packages.Clear();
    }

    public void Update()
    {
        if (!IsOpen)
        {
            return;
        }

        var isRunning = true;
        
        _sendPackagesWorker.Lock();
        if (!_sendPackagesWorker.IsRunning)
        {
            isRunning = false;
        }
        _sendPackagesWorker.Unlock();
        
        _receivePackagesWorker.Lock();
        if (!_receivePackagesWorker.IsRunning)
        {
            isRunning = false;
        }
        _receivePackagesWorker.Unlock();

        if (!isRunning)
        {
            Stop();
        }
        else
        {
            var diff = (int)(DateTime.Now - LastPackageDateTime).TotalSeconds;
            if (diff > MaxPackageDelay)
            {
                Send(new EmptyPackage());
            }
        }
    }
    
    public Event<TcpPackageEvent> this[NetworkPackageType type]
    {
        get
        {
            if (!_networkPackageEvent.ContainsKey(type))
            {
                _networkPackageEvent.Add(type, new Event<TcpPackageEvent>());
            }
        
            return _networkPackageEvent[type];
        }
    }
}