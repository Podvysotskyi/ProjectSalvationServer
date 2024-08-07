using System.Numerics;
using Game.Domain.Scene;
using Game.Helpers;

namespace Game.Network.Package.Types
{
    public class ServerPlayerScenePackage(Scene scene, Vector3? position, Quaternion? rotation) : NetworkPackage(NetworkPackageType.SPlayerScene)
    {
        private string Id => scene.Id;
        private Vector3 Position => position ?? scene.DefaultPosition;
        private Quaternion Rotation => rotation ?? scene.DefaultRotation;
        
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