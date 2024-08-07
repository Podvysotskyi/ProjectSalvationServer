using System.Numerics;
using Game.Domain.Player;
using Game.Helpers;

namespace Game.Network.Package.Types
{
    public class ServerPlayerPositionPackage(Player player) : NetworkPackage(NetworkPackageType.SPlayerPosition)
    {
        private string Id => player.Id;
        private Vector3 Position => MathHelper.Round(player.Transform.Position, 3);
        private Quaternion Rotation => MathHelper.Round(player.Transform.Rotation, 3);
        private float Speed => player.Speed;
        private Vector3 Movement => MathHelper.Round(player.Movement, 3);

        public override byte[] ToArray()
        {
            var result = new List<byte>();
            
            result.AddRange(BitConverterHelper.ToArray(Id));
            result.AddRange(BitConverterHelper.ToArray(Position));
            result.AddRange(BitConverterHelper.ToArray(Rotation));
            result.AddRange(BitConverterHelper.ToArray(Speed));
            result.AddRange(BitConverterHelper.ToArray(Movement));
            
            return result.ToArray();
        }
    }
}