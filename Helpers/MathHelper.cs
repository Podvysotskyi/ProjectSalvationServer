using System.Numerics;

namespace Game.Helpers;

public static class MathHelper
{
    public static Vector3 Round(Vector3 value, int digits)
    {
        var x = MathF.Round(value.X, digits);
        var y = MathF.Round(value.Y, digits);
        var z = MathF.Round(value.Z, digits);
        
        return new Vector3(x, y, z);
    }
    
    public static Quaternion Round(Quaternion value, int digits)
    {
        var x = MathF.Round(value.X, digits);
        var y = MathF.Round(value.Y, digits);
        var z = MathF.Round(value.Z, digits);
        var w = MathF.Round(value.W, digits);
        
        return new Quaternion(x, y, z, w);
    }
}