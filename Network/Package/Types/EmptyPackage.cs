namespace Game.Network.Package.Types
{
    public abstract class EmptyPackage() : NetworkPackage(NetworkPackageType.Empty)
    {
        public override byte[] ToArray()
        {
            return [];
        }
    }
}