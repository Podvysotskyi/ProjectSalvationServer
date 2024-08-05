namespace Game.Core;

public interface IService : Initializable
{
    public void Start();
    public void Stop();
}