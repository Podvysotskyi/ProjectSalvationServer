using Game.Core;
using Game.Network.Tcp;

namespace Game.Network;

public class NetworkService : IService
{
    public readonly TcpService Tcp;
    
    public NetworkService()
    {
        Tcp = new TcpService("localhost", 5555);
    }
    
    public void Init()
    {
        Tcp.Init();
    }

    public void Start()
    {
        Tcp.Start();
    }

    public void Stop()
    {
        Tcp.Stop();
    }
}