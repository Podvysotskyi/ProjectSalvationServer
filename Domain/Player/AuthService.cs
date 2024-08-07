using Game.Core;
using Game.Domain.User;
using Game.Engine;
using Game.Helpers;
using Game.Network.Package;
using Game.Network.Package.Types;
using Game.Network.Tcp.Events;

namespace Game.Domain.Player;

public class AuthService : IService
{
    public readonly Event<UserLoginEvent> UserLoginEvent = new();

    private UserRepository UserRepository => DomainManager.Repository<UserRepository>();
    
    public void Init()
    {
        NetworkManager.Tcp.ConnectionAcceptedEvent.AddListener(OnConnectionAccepted);
    }

    public void Start()
    {
    }

    public void Stop()
    {
    }

    private void OnConnectionAccepted(TcpConnectionEvent e)
    {
        var connection = e.Connection;
        
        connection[NetworkPackageType.CLogin].AddListener(OnLoginPackage);
    }

    private void OnLoginPackage(TcpPackageEvent e)
    {
        var package = NetworkPackageFacade.Convert<ClientLoginPackage>(e.Package);
        var connection = e.Connection;

        var user = UserRepository.Get(package.Login).FirstOrDefault();
        if (user == null)
        {
            user = UserRepository.Create(package.Login, package.Password);
        }
        else
        {
            var passwordHash = HashHelper.GetSha256Hash(package.Password);
            if (user.Password != passwordHash)
            {
                user = null;
            }
        }

        if (user == null)
        {
            connection.Send(new ServerPlayerAuthPackage(false));
            return;
        }
        
        connection[NetworkPackageType.CLogin].RemoveAllListeners();
        UserLoginEvent.Invoke(new UserLoginEvent(connection, user));
    }
}