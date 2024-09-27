using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

internal static class FastBinaryWriter
{
    public static void Write(Span<byte> buffer, ref int pos, int data)
    {
        BitConverter.TryWriteBytes(buffer[pos..], data);
        pos += 4;
    }

    public static void Write(Span<byte> buffer, ref int pos, uint data)
    {
        BitConverter.TryWriteBytes(buffer[pos..], data);
        pos += 4;
    }

    public static void Write(Span<byte> buffer, ref int pos, short data)
    {
        BitConverter.TryWriteBytes(buffer[pos..], data);
        pos += 2;
    }
    public static void Write(Span<byte> buffer, ref int pos, ushort data)
    {
        BitConverter.TryWriteBytes(buffer[pos..], data);
        pos += 2;
    }

    public static void Write(Span<byte> buffer, ref int pos, byte data, int advance = 1)
    {
        BitConverter.TryWriteBytes(buffer[pos..], data);
        pos += advance;
    }

    public static void Write(Span<byte> buffer, ref int pos, sbyte data)
    {
        BitConverter.TryWriteBytes(buffer[pos..], data);
        pos += 1;
    }

    public static void Write(Span<byte> buffer, ref int pos, bool data)
    {
        BitConverter.TryWriteBytes(buffer[pos..], data);
        pos += 1;
    }

    public static void Write(Span<byte> buffer, ref int pos, float data)
    {
        BitConverter.TryWriteBytes(buffer[pos..], data);
        pos += 4;
    }

    public static void Write(byte[] buffer, ref int pos, int data)
    {
        BitConverter.TryWriteBytes(buffer.AsSpan()[pos..], data);
        pos += 4;
    }

    public static void Write<T>(Span<byte> buffer, ref int pos, T dataStruct) where T : struct
    {
        MemoryMarshal.Write(buffer[pos..], ref dataStruct);
        pos += Unsafe.SizeOf<T>();
    }
}
