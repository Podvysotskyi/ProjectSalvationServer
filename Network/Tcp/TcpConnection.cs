using System.Net.Sockets;
using Game.Core;
using Game.Network.Package;
using Game.Network.Tcp.Events;
using Game.Network.Tcp.Workers;

namespace Game.Network.Tcp;

public class TcpConnection : IDisposable
{
    public readonly string Id;
        
    public bool IsOpen { get; private set; }
        
    private readonly Socket _socket;
    private readonly SendPackagesWorker _sendPackagesWorker;
    private readonly ReceivePackagesWorker _receivePackagesWorker;
    
    private readonly Dictionary<NetworkPackageType, Event<TcpPackageEvent>> _networkPackageEvent = new();

    public TcpConnection(Socket socket)
    {
        _socket = socket;
            
        IsOpen = true;
        Id = Guid.NewGuid().ToString();
            
        _sendPackagesWorker = new SendPackagesWorker(socket);
        _sendPackagesWorker.Start();

        _receivePackagesWorker = new ReceivePackagesWorker(socket);
        _receivePackagesWorker.Start();
    }
        
    public void Dispose()
    {
        Console.WriteLine($"{Id} | TCP network: stop");
            
        _sendPackagesWorker.Stop();
        _receivePackagesWorker.Stop();
            
        _sendPackagesWorker.Join();
        _receivePackagesWorker.Join();
            
        _socket.Shutdown(SocketShutdown.Both);
        _socket.Close();

        IsOpen = false;
    }

    public void Send(NetworkPackage package)
    {
        _sendPackagesWorker.Send(package);
        Console.WriteLine($"{Id} | TCP network: send package '{package.Type}'");
    }

    public void Receive()
    {
        var packages = _receivePackagesWorker.Receive();
        foreach (var package in packages)
        {
            Console.WriteLine($"{Id} | TCP network: received package '{package.Type}'");
            this[package.Type].Invoke(new TcpPackageEvent(this, package));
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