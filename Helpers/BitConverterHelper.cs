using System.Numerics;
using Game.Network.Package;

namespace Game.Helpers;

public static class BitConverterHelper
{
    public static string ReadString(byte[] data, ref int position)
    {
        var length = ReadUShort(data, ref position);

        if (length == 0)
        {
            return "";
        }

        var result = ReadBytes(data, length, ref position);

        return System.Text.Encoding.Unicode.GetString(result);
    }

    public static byte[] ReadBytes(byte[] data, int length, ref int position)
    {
        var result = new byte[length];
        Array.Copy(data, position, result, 0, length);
        position += length;
        return result;
    }
        
    public static ushort ReadUShort(byte[] data, ref int position)
    {
        var result = BitConverter.ToUInt16(data, position);
        position += 2;
        return result;
    }
    
    public static byte[] ToArray(bool value)
    {
        byte result = 0;
        if (value)
        {
            result = 1;
        }
        return [result];
    }

    public static byte[] ToArray(ushort value)
    {
        return BitConverter.GetBytes(value);
    }

    public static byte[] ToArray(NetworkPackageType type)
    {
        return ToArray((ushort)type);
    }

    public static byte[] ToArray(string value)
    {
        var buffer = new List<byte>();

        var data = System.Text.Encoding.Unicode.GetBytes(value);
        var length = (ushort)data.Length;
            
        buffer.AddRange(ToArray(length));

        if (length > 0)
        {
            buffer.AddRange(data);
        }
            
        return buffer.ToArray();
    }

    public static byte[] ToArray(Vector3 value)
    {
        var buffer = new List<byte>();

        buffer.AddRange(BitConverter.GetBytes(value.X));
        buffer.AddRange(BitConverter.GetBytes(value.Y));
        buffer.AddRange(BitConverter.GetBytes(value.Z));

        return buffer.ToArray();
    }
    
    public static byte[] ToArray(Quaternion value)
    {
        var buffer = new List<byte>();

        buffer.AddRange(BitConverter.GetBytes(value.X));
        buffer.AddRange(BitConverter.GetBytes(value.Y));
        buffer.AddRange(BitConverter.GetBytes(value.Z));
        buffer.AddRange(BitConverter.GetBytes(value.W));

        return buffer.ToArray();
    }
}