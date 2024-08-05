namespace Game.Network.Package.Types
{
    public class EmptyPackage() : NetworkPackage(NetworkPackageType.Empty)
    {
        public override byte[] ToArray()
        {
            return [];
        }
    }
}