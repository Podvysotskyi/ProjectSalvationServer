using System.Numerics;
using Game.Helpers;

namespace Game.Network.Package.Types
{
    public class ClientPlayerPositionPackage : NetworkPackage
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;
        public readonly Vector3 Movement;

        public ClientPlayerPositionPackage(byte[] data) : base(NetworkPackageType.CPlayerPosition)
        {
            var position = 0;
            
            Position = BitConverterHelper.ReadVector3(data, ref position);
            Rotation = BitConverterHelper.ReadQuaternion(data, ref position);
            Movement = BitConverterHelper.ReadVector3(data, ref position);
        }
    }
}