using Game.Database.Core;
using Game.Network.Domain.Enums;

namespace Game.Network.Domain.Packages
{
    public abstract class EmptyPackage() : Package(PackageType.Empty)
    {
        public override byte[] ToArray()
        {
            return [];
        }
    }
}