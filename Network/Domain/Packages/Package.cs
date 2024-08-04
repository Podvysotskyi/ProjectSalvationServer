using Game.Network.Domain.Enums;

namespace Game.Network.Domain.Packages
{
    public abstract class Package(PackageType type)
    {
        public PackageType Type { get; private set; } = type;

        public abstract byte[] ToArray();
    }
}