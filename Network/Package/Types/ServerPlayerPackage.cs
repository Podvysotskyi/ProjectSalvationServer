using Game.Domain.Player;
using Game.Helpers;

namespace Game.Network.Package.Types
{
    public class ServerPlayerPackage(PlayerEntity player, bool status) : NetworkPackage(NetworkPackageType.SPlayer)
    {
        private readonly string _id = player.Id;
        private readonly string _name = player.Name;

        public override byte[] ToArray()
        {
            var result = new List<byte>();
            
            result.AddRange(BitConverterHelper.ToArray(_id));
            result.AddRange(BitConverterHelper.ToArray(status));
            if (status)
            {
                result.AddRange(BitConverterHelper.ToArray(_name));
            }

            return result.ToArray();
        }
    }
}