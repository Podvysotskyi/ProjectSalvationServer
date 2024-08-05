using System.Numerics;
using Game.Domain.Player;
using Game.Network.Package.Types;

namespace Game.Domain.Scene;

public partial class SceneEntity
{
    private readonly Dictionary<string, PlayerEntity> _players = new();

    private void UpdatePlayerPositions()
    {
        foreach (var i in _players.Keys)
        {
            foreach (var j in _players.Keys)
            {
                if (_players[i].Id != _players[j].Id)
                {
                    _players[i].SendTcpPackage(new ServerPlayerPosition(_players[j]));
                }
            }
            _players[i].SendTcpPackage(new ServerPlayerPosition(_players[i]));
        }
    }
    
    public void AddPlayer(PlayerEntity player, Vector3 position)
    {
        player.Scene?.RemovePlayer(player);
        
        player.Scene = this;
        player.Transform.Position = position;
        player.Transform.Rotation = DefaultRotation;
        
        //TODO: send User Update Scene event
        foreach (var i in _players.Keys)
        {
            _players[i].SendTcpPackage(new ServerPlayerStatusPackage(player, true));
        }
        _players.Add(player.Id, player);
    }

    public void AddPlayer(PlayerEntity player)
    {
        AddPlayer(player, DefaultPosition);
    }

    public void RemovePlayer(PlayerEntity player)
    {
        player.Scene = null;
        _players.Remove(Id);
        
        foreach (var i in _players.Keys)
        {
            _players[i].SendTcpPackage(new ServerPlayerStatusPackage(player, false));
        }
        //TODO: send User Update Scene event
    }
}