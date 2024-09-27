using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook;

internal static class FastBinaryWriter
{
    public static void Write(Span<byte> buffer, ref int pos, int data)
    {
        MemoryMarshal.Write(buffer[pos..], ref data);
        pos += 4;
    }

    public static void Write(Span<byte> buffer, ref int pos, uint data)
    {
        MemoryMarshal.Write(buffer[pos..], ref data);
        pos += 4;
    }

    public static void Write(Span<byte> buffer, ref int pos, short data)
    {
        MemoryMarshal.Write(buffer[pos..], ref data);
        pos += 2;
    }
    public static void Write(Span<byte> buffer, ref int pos, ushort data)
    {
        MemoryMarshal.Write(buffer[pos..], ref data);
        pos += 2;
    }

    public static void Write(Span<byte> buffer, ref int pos, byte data, int advance = 1)
    {
        MemoryMarshal.Write(buffer[pos..], ref data);
        pos += advance;
    }

    public static void Write(Span<byte> buffer, ref int pos, sbyte data)
    {
        MemoryMarshal.Write(buffer[pos..], ref data);
        pos += 1;
    }

    public static void Write(Span<byte> buffer, ref int pos, bool data)
    {
        MemoryMarshal.Write(buffer[pos..], ref data);
        pos += 1;
    }

    public static void Write(Span<byte> buffer, ref int pos, float data)
    {
        MemoryMarshal.Write(buffer[pos..], ref data);
        pos += 4;
    }

    public static void Write(byte[] buffer, ref int pos, int data)
    {
        MemoryMarshal.Write(buffer.AsSpan()[pos..], ref data);
        pos += 4;
    }
}
