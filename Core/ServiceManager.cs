namespace Game.Core;

public abstract class ServiceManager<T> where T : IService
{
    private static T? _instance;
    public static T Instance => _instance ??= Activator.CreateInstance<T>();

    public static void Init()
    {
        Console.WriteLine($"{typeof(T).Name}: init");
        
        Instance.Init();
    }
    
    public static void Start()
    {
        Console.WriteLine($"{typeof(T).Name}: start");
        
        Instance.Start();
    }
    
    public static void Stop()
    {
        Console.WriteLine($"{typeof(T).Name}: stop");
        
        Instance.Stop();
    }
}