using Game.Core;
using Game.Domain.Scene;

namespace Game.Engine;

public class SceneManager : ServiceManager<SceneService>
{
    public static SceneEntity DefaultScene => Scene("welcome");
    
    public static SceneEntity Scene(string id)
    {
        return Instance.GetScene(id);
    }

    public static void Update()
    {
        Instance.Update();
    }
}