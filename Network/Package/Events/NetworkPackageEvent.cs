namespace Game.Network.Package.Events;

public class NetworkPackageEvent(NetworkPackage package)
{
    public NetworkPackageType Type => Package.Type;
    public readonly NetworkPackage Package = package;
}