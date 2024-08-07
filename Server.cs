using Game.Core;
using Game.Database;
using Game.Engine;

namespace Game;

public class Server : IService
{
    private bool _running;
    private readonly Mutex _mutex;
    private readonly Thread _thread;

    public Server()
    {
        _running = true;
        _mutex = new Mutex();
        _thread = new Thread(Run);
    }

    public void Init()
    {
        DatabaseService.Instance.Init();
        DomainManager.Init();
        SceneManager.Init();
        PlayerManager.Init();
        NetworkManager.Init();
    }

    public void Start()
    {
        DatabaseService.Instance.Start();
        DomainManager.Start();
        SceneManager.Start();
        PlayerManager.Start();
        NetworkManager.Start();

        _thread.Start();
    }

    public void Stop()
    {
        _mutex.WaitOne();
        _running = false;
        _mutex.ReleaseMutex();

        _thread.Join();

        PlayerManager.Stop();
        SceneManager.Stop();

        NetworkManager.Stop();
    }

    private void Run()
    {
        const float fps = 40.0f;
        
        var targetTime = 1000 / fps;
        var time = DateTime.Now;

        while (true)
        {
            _mutex.WaitOne();
            if (!_running)
            {
                _mutex.ReleaseMutex();
                break;
            }

            _mutex.ReleaseMutex();

            {
                NetworkManager.Update();
                PlayerManager.Update(targetTime / 1000.0f);
                
                //TODO: Update physics
                
                PlayerManager.SendPositionUpdates();
                SceneManager.SendPositionUpdates();
            }

            var frameTime = (DateTime.Now - time).TotalMilliseconds;
            if (frameTime < targetTime)
            {
                Thread.Sleep((int)(targetTime - frameTime));
            }

            time = DateTime.Now;
        }
    }
}