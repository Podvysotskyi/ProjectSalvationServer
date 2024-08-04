using Game.Network.Domain.Enums;
using Game.Network.Domain.Packages;

namespace Game.Network.Events
{
    public abstract class NetworkPackageEvent(Package package)
    {
        public PackageType Type => Package.Type;
        public readonly Package Package = package;
    }
}