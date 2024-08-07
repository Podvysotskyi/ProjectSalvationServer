using System.Numerics;
using Game.Domain.Entity;
using Game.Domain.User;
using Game.Helpers;
using Game.Network.Package;
using Game.Network.Package.Events;
using Game.Network.Package.Types;
using Game.Network.Tcp;

namespace Game.Domain.Player;

public partial class Player : GameObject
{
    private readonly UserModel _user;
    private readonly TcpConnection _connection;

    public string Id => _user.Id;
    public string Name => _user.Login;

    public float Speed { get; private set; } = 2;

    private Vector3 _movement = Vector3.Zero;

    public Vector3 Movement
    {
        get => Scene == null ? Vector3.Zero : _movement;
        private set => _movement = value;
    }

    public Player(UserModel user, TcpConnection connection)
    {
        _user = user;
        _connection = connection;

        connection[NetworkPackageType.CSceneReady].AddListener(OnSceneReadyPackage);
        connection[NetworkPackageType.CPlayerPosition].AddListener(OnPlayerPositionPackage);
    }

    public override void Dispose()
    {
        Scene?.RemovePlayer(Id);
    }

    public override void Update(float delta)
    {
        
        Transform.Position += Movement * Speed * delta;

        //TODO: player logic
    }

    private void OnPlayerPositionPackage(NetworkPackageEvent e)
    {
        var package = NetworkPackageFacade.Convert<ClientPlayerPositionPackage>(e.Package);

        if (Scene != null)
        {
            Transform.Rotation = MathHelper.Round(package.Rotation, 3);
            _movement = MathHelper.Round(package.Movement, 3);
        }
    }
    
    public void SendTcpPackage(NetworkPackage package)
    {
        _connection.Send(package);
    }
}