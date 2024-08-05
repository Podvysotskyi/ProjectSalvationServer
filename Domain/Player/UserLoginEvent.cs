using Game.Domain.User;
using Game.Network.Tcp;

namespace Game.Domain.Player;

public class UserLoginEvent(TcpConnection connection, UserModel user)
{
    public readonly TcpConnection Connection = connection;
    public readonly UserModel User = user;
}