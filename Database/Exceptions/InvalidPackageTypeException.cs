using Game.Database.Core;
using Game.Network.Domain;
using Game.Network.Domain.Enums;

namespace Game.Database.Exceptions
{
    public class InvalidPackageTypeException(PackageType type) : Exception("Invalid package type")
    {
        public readonly PackageType Type = type;
    }
}
