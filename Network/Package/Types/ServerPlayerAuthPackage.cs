using Game.Domain.Player;
using Game.Helpers;

namespace Game.Network.Package.Types
{
    public class ServerPlayerAuthPackage(PlayerEntity player, bool status) : NetworkPackage(NetworkPackageType.SAuth)
    {
        public override byte[] ToArray()
        {
            var result = new List<byte>();
            
            result.AddRange(BitConverterHelper.ToArray(status));

            if (status)
            {
                result.AddRange(BitConverterHelper.ToArray(player.Id));
                result.AddRange(BitConverterHelper.ToArray(player.Name));
            }

            return result.ToArray();
        }
    }
}