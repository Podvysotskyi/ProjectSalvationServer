using Game.Core;
using Game.Domain.Scene;

namespace Game.Engine;

public class SceneManager : ServiceManager<SceneService>
{
    public static ref Scene DefaultScene => ref Scene("welcome");
    
    public static ref Scene Scene(string id)
    {
        return ref Instance.GetScene(id);
    }

    public static void SendPositionUpdates()
    {
        Instance.SendPositionUpdates();
    }
}