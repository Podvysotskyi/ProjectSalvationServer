using Game.Domain.Entity;
using Game.Domain.User;
using Game.Network.Package;
using Game.Network.Tcp;

namespace Game.Domain.Player;

public class PlayerEntity(UserModel user, TcpConnection connection) : GameObject
{
    public string Id => user.Id;
    public string Name => user.Login;

    public void SendTcpPackage(NetworkPackage package)
    {
        connection.Send(package);
    }
}