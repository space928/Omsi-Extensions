using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHookRPCPlugin;

internal static class FastBinaryReader
{
    public static int ReadI32(Span<byte> buffer, ref int pos)
    {
        var ret = MemoryMarshal.Read<int>(buffer[pos..]);
        pos += 4;
        return ret;
    }

    public static uint ReadU32(Span<byte> buffer, ref int pos)
    {
        var ret = MemoryMarshal.Read<uint>(buffer[pos..]);
        pos += 4;
        return ret;
    }

    public static short ReadI16(Span<byte> buffer, ref int pos)
    {
        var ret = MemoryMarshal.Read<short>(buffer[pos..]);
        pos += 2;
        return ret;
    }

    public static ushort ReadU16(Span<byte> buffer, ref int pos)
    {
        var ret = MemoryMarshal.Read<ushort>(buffer[pos..]);
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
        var ret = MemoryMarshal.Read<bool>(buffer[pos..]);
        pos += 1;
        return ret;
    }

    public static float ReadFloat(Span<byte> buffer, ref int pos)
    {
        var ret = MemoryMarshal.Read<float>(buffer[pos..]);
        pos += 4;
        return ret;
    }
}
