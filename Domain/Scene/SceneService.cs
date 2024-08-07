using Game.Core;

namespace Game.Domain.Scene;

public class SceneService : IService
{
    private readonly Scene?[] _scenes = new Scene[10];

    public void Init()
    {
        for (var i = 0; i < _scenes.Length; i++)
        {
            _scenes[i] = null;
        }
        
        LoadScene("welcome");
    }
    
    public void Start()
    {
        for (var i = 0; i < _scenes.Length; i++)
        {
            if (_scenes[i] == null)
            {
                continue;
            }
            
            Console.WriteLine($"{GetType().Name}: load scene '{_scenes[i]!.Id}'");
            _scenes[i]!.Load();
        }
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public void SendPositionUpdates()
    {
        for (var i = 0; i < _scenes.Length; i++)
        {
            _scenes[i]?.SendPositionUpdates();
        }
    }

    private ref Scene LoadScene(string id)
    {
        for (var i = 0; i < _scenes.Length; i++)
        {
            if (_scenes[i] != null)
            {
                continue;
            }

            _scenes[i] = new Scene(id);
            return ref _scenes[i]!;
        }

        throw new Exception();
    }

    public ref Scene GetScene(string id)
    {
        for (var i = 0; i < _scenes.Length; i++)
        {
            if (_scenes[i] != null && _scenes[i]!.Id == id)
            {
                return ref _scenes[i]!;
            }
        }

        throw new Exception();
    }
}