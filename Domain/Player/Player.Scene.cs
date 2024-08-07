using System.Numerics;
using Game.Engine;
using Game.Network.Package;
using Game.Network.Package.Events;
using Game.Network.Package.Types;

namespace Game.Domain.Player;

public partial class Player
{
    private string? _targetScene;
    private Scene.Scene? TargetScene => _targetScene == null ? null : SceneManager.Scene(_targetScene);
    private Vector3? _targetScenePosition;
    private Quaternion? _targetSceneRotation;
    
    public void MoveToScene(string id, Vector3? position = null, Quaternion? rotation = null)
    {
        if (_targetScene == id || Scene?.Id == id)
        {
            return;
        }
        
        Movement = Vector3.Zero;

        _targetScene = id;
        _targetScenePosition = position ?? TargetScene!.DefaultPosition;
        _targetSceneRotation = rotation ?? TargetScene!.DefaultRotation;
        
        Scene?.RemovePlayer(Id);
        SendTcpPackage(new ServerPlayerScenePackage(SceneManager.Scene(id), _targetScenePosition, _targetSceneRotation));
    }

    private void OnSceneReadyPackage(NetworkPackageEvent e)
    {
        var package = NetworkPackageFacade.Convert<ClientSceneReadyPackage>(e.Package);

        if (package.Id == Scene?.Id || package.Id != _targetScene)
        {
            return;
        }
        
        TargetScene?.AddPlayer(Id, _targetScenePosition, _targetSceneRotation);
    }
}