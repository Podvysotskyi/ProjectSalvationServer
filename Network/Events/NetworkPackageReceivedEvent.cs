using Game.Network.Domain.Packages;

namespace Game.Network.Events
{
    public class NetworkPackageReceivedEvent(Package package) : NetworkPackageEvent(package);
}