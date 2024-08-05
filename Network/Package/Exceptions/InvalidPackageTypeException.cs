namespace Game.Network.Package.Exceptions;

public class InvalidPackageTypeException(NetworkPackageType type) : Exception("Invalid package type")
{
    public readonly NetworkPackageType Type = type;
}