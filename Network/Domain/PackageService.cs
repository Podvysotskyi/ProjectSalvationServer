using Game.Core;
using Game.Database.Exceptions;
using Game.Network.Domain.Enums;
using Game.Network.Domain.Packages;

namespace Game.Network.Domain
{
    public class PackageService : IService
    {
        private static PackageService? _instance;
        public static PackageService Instance => _instance ??= new PackageService();

        private readonly Dictionary<PackageType, Type> _packages = new();
        
        public void Init()
        {
            Register<EmptyPackage>(PackageType.Empty);
            Register<ClientLoginPackage>(PackageType.CLogin);
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private void Register<T>(PackageType type) where T : Package
        {
            _packages.Add(type, typeof(T));
        }
        
        public Package? CreatePackage(PackageType packageType)
        {
            if (!_packages.TryGetValue(packageType, out var type))
            {
                throw new InvalidPackageTypeException(packageType);
            }

            return Activator.CreateInstance(type) as Package;
        }

        public Package? CreatePackage(PackageType packageType, byte[] data)
        {
            if (!_packages.TryGetValue(packageType, out var type))
            {
                throw new InvalidPackageTypeException(packageType);
            }

            return Activator.CreateInstance(type, data) as Package;
        }
    }
}