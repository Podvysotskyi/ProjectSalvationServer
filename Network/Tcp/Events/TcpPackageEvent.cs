using Game.Network.Package;
using Game.Network.Package.Events;

namespace Game.Network.Tcp.Events;

public class TcpPackageEvent(TcpConnection connection, NetworkPackage package) : NetworkPackageEvent(package)
{
    public readonly TcpConnection Connection = connection;
}