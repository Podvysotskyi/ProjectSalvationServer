using Game.Core;
using Game.Network;
using Game.Network.Tcp;

namespace Game.Engine;

public class NetworkManager : ServiceManager<NetworkService>
{
    public static TcpService Tcp => Instance.Tcp;
}