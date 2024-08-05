using Game.Domain.Scene;

namespace Game.Domain.Entity;

public class GameObject
{
    public readonly Transform Transform;

    public SceneEntity? Scene;

    public GameObject()
    {
        Transform = new Transform();
    }
}