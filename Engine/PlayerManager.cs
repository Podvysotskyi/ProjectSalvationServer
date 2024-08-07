using Game.Core;
using Game.Domain.Player;

namespace Game.Engine;

public class PlayerManager : ServiceManager<PlayerService>
{
    public static ref Player Player(string id)
    {
        return ref Instance.GetPlayer(id);
    }
    
    public static void Update(float delta)
    {
        Instance.Update(delta);
    }

    public static void SendPositionUpdates()
    {
        Instance.SendPositionUpdates();
    }
}