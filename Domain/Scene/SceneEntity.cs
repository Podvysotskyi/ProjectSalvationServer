namespace Game.Domain.Scene;

public partial class SceneEntity(string id)
{
    public readonly string Id = id;

    public void Load()
    {
        //TODO: read scene from file
    }

    public void Update()
    {
        UpdatePlayerPositions();
    }
}