namespace Game.Domain;

public abstract class Model
{
    public readonly string Id;

    protected Model(string id)
    {
        Id = id;
    }
    
    protected Model()
    {
        Id = Guid.NewGuid().ToString();
    }
}