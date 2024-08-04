using Game.Network.Domain.Packages;

namespace Game.Network.Events
{ 
    public class NetworkPackageSentEvent(Package package) : NetworkPackageEvent(package);
}