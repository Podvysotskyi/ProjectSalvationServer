using Game.Helpers;

namespace Game.Network.Package.Types
{
    public class ClientLoginPackage : NetworkPackage
    {
        public readonly string Login;
        public readonly string Password;

        public ClientLoginPackage(byte[] data) : base(NetworkPackageType.CLogin)
        {
            var position = 0;
            
            Login = BitConverterHelper.ReadString(data, ref position);
            Password = BitConverterHelper.ReadString(data, ref position);
        }
    }
}