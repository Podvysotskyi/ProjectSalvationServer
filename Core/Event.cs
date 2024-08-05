namespace Game.Core;

public class Event<T>
{
    private event EventHandler<T> _event;

    private readonly List<Action<T>> _listeners;

    public Event()
    {
        _listeners = new List<Action<T>>();

        _event += EventHandler;
    }

    public void Invoke(T e)
    {
        _event.Invoke(this, e);
    }
    
    public void AddListener(Action<T> action)
    {
        _listeners.Add(action);
    }

    public void RemoveListener(Action<T> action)
    {
        _listeners.Remove(action);
    }

    public void RemoveAllListeners()
    {
        _listeners.Clear();
    }

    private void EventHandler(object? target, T e)
    {
        for (var i = 0; i < _listeners.Count; i++) {
            _listeners[i].Invoke(e);
        }
    }
}