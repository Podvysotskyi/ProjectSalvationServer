using Game.Core;

namespace Game.Domain.Scene;

public class SceneService : IService
{
    private readonly Dictionary<string, SceneEntity> _scenes;

    public SceneService()
    {
        _scenes = new Dictionary<string, SceneEntity>();
    }

    public void Init()
    {
        LoadScene("welcome");
    }
    
    public void Start()
    {
        foreach (var scene in _scenes.Values)
        {
            Console.WriteLine($"{GetType().Name}: load scene '{scene.Id}'");
            scene.Load();
        }
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        foreach (var scene in _scenes.Values)
        {
            scene.Update();
        }
    }

    private SceneEntity LoadScene(string id)
    {
        var scene = new SceneEntity(id);
        _scenes.Add(id, scene);
        return scene;
    }

    public SceneEntity GetScene(string id)
    {
        if (!_scenes.TryGetValue(id, out var scene))
        {
            throw new Exception(); //TODO: add exception class
        }

        return scene;
    }
}