using System.Numerics;
using Game.Engine;
using Game.Network.Package.Types;

namespace Game.Domain.Scene;

public partial class Scene
{
    private readonly List<string> _players = new();

    public void SendPositionUpdates()
    {
        foreach (var i in _players)
        {
            foreach (var j in _players)
            {
                if (i != j)
                {
                    PlayerManager.Player(i).SendTcpPackage(
                        new ServerPlayerPositionPackage(PlayerManager.Player(j))
                    );
                }
            }
        }
    }
    
    public void AddPlayer(string playerId, Vector3? position, Quaternion? rotation)
    {
        var player = PlayerManager.Player(playerId);
        if (player.Scene?.Id == Id)
        {
            return;
        }
        
        player.Scene?.RemovePlayer(playerId);
        player.Scene = this;
        player.Transform.Position = position ?? DefaultPosition;
        player.Transform.Rotation = rotation ?? DefaultRotation;
        
        foreach (var id in _players)
        {
            var scenePlayer = PlayerManager.Player(id);
            scenePlayer.SendTcpPackage(new ServerPlayerStatusPackage(player, true));
            player.SendTcpPackage(new ServerPlayerStatusPackage(scenePlayer, true));
        }
        _players.Add(playerId);
    }

    public void RemovePlayer(string playerId)
    {
        var player = PlayerManager.Player(playerId);
        player.Scene = null;
        _players.Remove(playerId);
        
        foreach (var id in _players)
        {
            var scenePlayer = PlayerManager.Player(id);
            scenePlayer.SendTcpPackage(new ServerPlayerStatusPackage(player, false));
            player.SendTcpPackage(new ServerPlayerStatusPackage(scenePlayer, false));
        }
    }
}