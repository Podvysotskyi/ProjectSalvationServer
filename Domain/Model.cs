namespace Game.Domain;

public abstract class Model
{
    public string Id { get; protected set; } = Guid.NewGuid().ToString();
}