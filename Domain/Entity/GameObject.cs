using Game.Engine;

namespace Game.Domain.Entity;

public class GameObject : IDisposable
{
    public readonly Transform Transform = new();

    private string? _scene;

    public Scene.Scene? Scene
    {
        get => _scene == null ? null : SceneManager.Scene(_scene);
        set => _scene = value?.Id;
    }

    public virtual void Update(float delta)
    {
    }

    public virtual void Dispose()
    {
    }
}