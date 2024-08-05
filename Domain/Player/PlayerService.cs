using Game.Core;
using Game.Engine;
using Game.Network.Package.Types;

namespace Game.Domain.Player;

public class PlayerService : IService
{
    private readonly AuthService _authService = new();

    public void Init()
    {
        _authService.Init();
        _authService.UserLoginEvent.AddListener(OnUserLogin);
    }

    public void Start()
    {
        _authService.Start();
    }

    public void Stop()
    {
        _authService.Stop();
    }

    private void OnUserLogin(UserLoginEvent e)
    {
        var player = new PlayerEntity(e.User, e.Connection);
        
        player.SendTcpPackage(new ServerPlayerAuthPackage(player, true));
        
        SceneManager.DefaultScene.AddPlayer(player);
    }
}