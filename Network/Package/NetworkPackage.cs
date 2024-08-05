namespace Game.Network.Package
{
    public abstract class NetworkPackage
    {
        public NetworkPackageType Type { get; private set; }

        protected NetworkPackage(NetworkPackageType type)
        {
            Type = type;
        }

        public virtual byte[] ToArray()
        {
            throw new NotImplementedException();
        }
    }
}