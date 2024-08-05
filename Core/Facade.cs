namespace Game.Core;

public abstract class Facade<T>
{
    private static T? _instance;
    protected static T Instance => _instance ??= Activator.CreateInstance<T>();
}