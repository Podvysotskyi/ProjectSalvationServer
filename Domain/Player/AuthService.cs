using Game.Core;
using Game.Domain.User;
using Game.Engine;
using Game.Network.Package;
using Game.Network.Package.Types;
using Game.Network.Tcp.Events;

namespace Game.Domain.Player;

public class AuthService : IService
{
    public readonly Event<UserLoginEvent> UserLoginEvent = new();
    
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
        
        var user = new UserModel(package.Login, package.Password);

        var connection = e.Connection;
        connection[NetworkPackageType.CLogin].RemoveAllListeners();
        
        UserLoginEvent.Invoke(new UserLoginEvent(connection, user));
    }
}