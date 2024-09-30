using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

internal static class FastBinaryReader
{
    public static int ReadI32(Span<byte> buffer, ref int pos)
    {
        var ret = BitConverter.ToInt32(buffer[pos..]);
        pos += 4;
        return ret;
    }

    public static uint ReadU32(Span<byte> buffer, ref int pos)
    {
        var ret = BitConverter.ToUInt32(buffer[pos..]);
        pos += 4;
        return ret;
    }

    public static short ReadI16(Span<byte> buffer, ref int pos)
    {
        var ret = BitConverter.ToInt16(buffer[pos..]);
        pos += 2;
        return ret;
    }

    public static ushort ReadU16(Span<byte> buffer, ref int pos)
    {
        var ret = BitConverter.ToUInt16(buffer[pos..]);
        pos += 2;
        return ret;
    }

    public static sbyte ReadI8(Span<byte> buffer, ref int pos)
    {
        var ret = (sbyte)buffer[pos];
        pos += 1;
        return ret;
    }

    public static byte ReadU8(Span<byte> buffer, ref int pos)
    {
        var ret = buffer[pos];
        pos += 1;
        return ret;
    }

    public static bool ReadBool(Span<byte> buffer, ref int pos)
    {
        var ret = BitConverter.ToBoolean(buffer[pos..]);
        pos += 1;
        return ret;
    }

    public static float ReadFloat(Span<byte> buffer, ref int pos)
    {
        var ret = BitConverter.ToSingle(buffer[pos..]);
        pos += 4;
        return ret;
    }

    public static T Read<T>(Span<byte> buffer, ref int pos) where T : struct
    {
        var ret = MemoryMarshal.Read<T>(buffer[pos..]);
        pos += Unsafe.SizeOf<T>();
        return ret;
    }
}
