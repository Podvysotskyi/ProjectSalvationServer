using System.Numerics;
using Game.Domain.Player;
using Game.Helpers;

namespace Game.Network.Package.Types
{
    public class ServerPlayerPosition(PlayerEntity player) : NetworkPackage(NetworkPackageType.SPlayerPosition)
    {
        private string Id => player.Id;
        private Vector3 Position => player.Transform.Position;
        private Quaternion Rotation => player.Transform.Rotation;

        public override byte[] ToArray()
        {
            var result = new List<byte>();
            
            result.AddRange(BitConverterHelper.ToArray(Id));
            result.AddRange(BitConverterHelper.ToArray(Position));
            result.AddRange(BitConverterHelper.ToArray(Rotation));
            
            return result.ToArray();
        }
    }
}