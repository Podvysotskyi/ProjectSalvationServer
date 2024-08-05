using Game.Network.Package.Types;

namespace Game.Network.Package;

public class NetworkPackageService
{
    public readonly Dictionary<NetworkPackageType, Type> Packages;
    
    public NetworkPackageService()
    {
        Packages = new Dictionary<NetworkPackageType, Type>();
        
        RegisterPackageType<EmptyPackage>(NetworkPackageType.Empty);
        RegisterPackageType<ClientLoginPackage>(NetworkPackageType.CLogin);
    }

    private void RegisterPackageType<T>(NetworkPackageType type) where T : NetworkPackage
    {
        Packages.Add(type, typeof(T));
    }
}