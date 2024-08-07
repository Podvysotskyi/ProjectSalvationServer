using Game.Helpers;

namespace Game.Network.Package.Types
{
    public class ClientSceneReadyPackage : NetworkPackage
    {
        public readonly string Id;

        public ClientSceneReadyPackage(byte[] data) : base(NetworkPackageType.CSceneReady)
        {
            var position = 0;
            
            Id = BitConverterHelper.ReadString(data, ref position);
        }
    }
}