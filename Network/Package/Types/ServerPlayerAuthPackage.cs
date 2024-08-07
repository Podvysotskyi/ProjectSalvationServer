using Game.Domain.Player;
using Game.Helpers;

namespace Game.Network.Package.Types
{
    public class ServerPlayerAuthPackage(bool status, Player? player = null) : NetworkPackage(NetworkPackageType.SAuth)
    {
        private string Id => player?.Id ?? "";
        private string Name => player?.Name ?? "";
        
        public override byte[] ToArray()
        {
            var result = new List<byte>();
            
            result.AddRange(BitConverterHelper.ToArray(status));

            if (status)
            {
                result.AddRange(BitConverterHelper.ToArray(Id));
                result.AddRange(BitConverterHelper.ToArray(Name));
            }

            return result.ToArray();
        }
    }
}