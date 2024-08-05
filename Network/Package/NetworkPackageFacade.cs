using Game.Core;
using Game.Network.Package.Exceptions;

namespace Game.Network.Package;

public class NetworkPackageFacade : Facade<NetworkPackageService>
{
    public static NetworkPackage CreatePackage(NetworkPackageType networkPackageType)
    {
        if (!Instance.Packages.TryGetValue(networkPackageType, out var type))
        {
            throw new InvalidPackageTypeException(networkPackageType);
        }

        return Activator.CreateInstance(type) as NetworkPackage ?? throw new InvalidOperationException();
    }

    public static NetworkPackage CreatePackage(NetworkPackageType networkPackageType, byte[] data)
    {
        if (!Instance.Packages.TryGetValue(networkPackageType, out var type))
        {
            throw new InvalidPackageTypeException(networkPackageType);
        }

        return Activator.CreateInstance(type, data) as NetworkPackage ?? throw new InvalidOperationException();
    }

    public static T Convert<T>(NetworkPackage package) where T : NetworkPackage
    {
        if (!Instance.Packages.TryGetValue(package.Type, out var type))
        {
            throw new InvalidPackageTypeException(package.Type);
        }

        if (typeof(T) != type)
        {
            throw new InvalidPackageTypeException(package.Type);
        }

        return package as T ?? throw new InvalidOperationException();
    }
}