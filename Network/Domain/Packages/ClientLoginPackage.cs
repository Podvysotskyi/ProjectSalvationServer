using Game.Database.Core;
using Game.Helpers;
using Game.Network.Domain.Enums;

namespace Game.Network.Domain.Packages
{
    public class ClientLoginPackage() : Package(PackageType.CLogin)
    {
        public string Login { get; private set; }
        public string Password { get; private set; }

        public ClientLoginPackage(string login, string password) : this()
        {
            Login = login;
            Password = password;
        }

        public ClientLoginPackage(byte[] data) : this()
        {
            var position = 0;
            
            Login = BitConverterHelper.ReadString(data, ref position);
            Password = BitConverterHelper.ReadString(data, ref position);
        }

        public override byte[] ToArray()
        {
            var result = new List<byte>();

            result.AddRange(BitConverterHelper.ToArray(Login));
            result.AddRange(BitConverterHelper.ToArray(Password));
            
            return result.ToArray();
        }
    }
}