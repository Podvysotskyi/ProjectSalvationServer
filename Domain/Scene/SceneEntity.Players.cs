using Game.Domain.Player;
using Game.Network.Package.Types;

namespace Game.Domain.Scene;

public partial class SceneEntity
{
    private readonly Dictionary<string, PlayerEntity> _players = new();

    private void UpdatePlayerPositions()
    {
        foreach (var client in _players.Values)
        {
            foreach (var player in _players.Values.Where(player => client.Id != player.Id))
            {
                client.SendTcpPackage(new ServerPlayerPosition(player));
            }
        }
    }
    
    public void AddPlayer(PlayerEntity newPlayer)
    {
        newPlayer.Scene?.RemovePlayer(newPlayer.Id);
        newPlayer.Scene = this;
        
        _players.Add(newPlayer.Id, newPlayer);

        foreach (var player in _players.Values)
        {
            player.SendTcpPackage(new ServerPlayerPackage(player, true));
        }
    }

    public void RemovePlayer(string id)
    {
        _players.Remove(id);
        
        foreach (var player in _players.Values)
        {
            player.SendTcpPackage(new ServerPlayerPackage(player, false));
        }
    }
}