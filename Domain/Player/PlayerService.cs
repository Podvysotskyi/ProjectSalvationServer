using Game.Core;
using Game.Domain.User;
using Game.Engine;
using Game.Network.Package.Types;
using Game.Network.Tcp;

namespace Game.Domain.Player;

public class PlayerService : IService
{
    private readonly AuthService _authService = new();

    private readonly Player?[] _players = new Player[100];

    public PlayerService()
    {
        for (var i = 0; i < _players.Length; i++)
        {
            _players[i] = null;
        }
    }

    private ref Player CreatePlayer(UserModel user, TcpConnection connection)
    {
        if (HasPlayer(user.Id))
        {
            throw new Exception();
        }

        for (var i = 0; i < _players.Length; i++)
        {
            if (_players[i] != null)
            {
                continue;
            }

            _players[i] = new Player(user, connection);
            return ref _players[i]!;
        }

        throw new Exception();
    }

    public ref Player GetPlayer(string id)
    {
        for (var i = 0; i < _players.Length; i++)
        {
            if (_players[i] != null && _players[i]!.Id == id)
            {
                return ref _players[i]!;
            }
        }

        throw new Exception();
    }

    public bool HasPlayer(string id)
    {
        for (var i = 0; i < _players.Length; i++)
        {
            if (_players[i]?.Id == id)
            {
                return true;
            }
        }

        return false;
    }

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

    public void Update(float delta)
    {
        for (var i = 0; i < _players.Length; i++)
        {
            _players[i]?.Update(delta);
        }
    }

    public void SendPositionUpdates()
    {
        for (var i = 0; i < _players.Length; i++)
        {
            if (_players[i] != null && _players[i]?.Scene != null)
            {
                _players[i]!.SendTcpPackage(new ServerPlayerPositionPackage(_players[i]!));
            }
        }
    }

    private void OnUserLogin(UserLoginEvent e)
    {
        var player = CreatePlayer(e.User, e.Connection);

        e.Connection.ConnectionClosedEvent.AddListener(_ => OnConnectionClosed(player.Id));

        player.SendTcpPackage(new ServerPlayerAuthPackage(true, player));
        player.MoveToScene(SceneManager.DefaultScene.Id);
    }

    private void OnConnectionClosed(string id)
    {
        for (var i = 0; i < _players.Length; i++)
        {
            if (_players[i]?.Id != id)
            {
                continue;
            }

            _players[i]?.Dispose();
            _players[i] = null;
            break;
        }
    }
}