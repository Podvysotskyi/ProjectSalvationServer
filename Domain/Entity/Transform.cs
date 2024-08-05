using System.Numerics;

namespace Game.Domain.Entity;

public class Transform(Vector3 position, Quaternion rotation)
{
    public Vector3 Position = position;
    public Quaternion Rotation = rotation;

    public Transform() : this(new Vector3(0, 0, 0), Quaternion.Identity)
    {
    }
    
    public Transform(Vector3 position) : this(position, Quaternion.Identity)
    {
    }
    
    public Transform(Quaternion rotation) : this(new Vector3(0, 0, 0), rotation)
    {
    }
}