namespace Game.Domain;

public abstract class Model(string id)
{
    public readonly string Id = id;

    protected Model() : this(Guid.NewGuid().ToString())
    {
    }
}