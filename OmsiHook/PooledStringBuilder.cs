using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook;

#nullable enable
#pragma warning disable CS9191

/// <summary>
/// A simple and fast string builder, which avoids memory allocations by using pooled arrays.
/// </summary>
public struct PooledStringBuilder : IDisposable
{
    private readonly ArrayPool<char> arrayPool;
    private char[] buff;
    private int pos;

    public PooledStringBuilder(int capacity = 16, ArrayPool<char>? arrayPool = null)
    {
        this.arrayPool = arrayPool ?? ArrayPool<char>.Shared;
        buff = this.arrayPool.Rent(capacity);
        pos = 0;
    }

    private void GrowIfNeeded(int charsToAdd)
    {
        if (charsToAdd + pos <= buff.Length)
            return;

        uint nsize = BitOperations.RoundUpToPowerOf2((uint)(charsToAdd + pos));
        var nbuff = arrayPool.Rent((int)nsize);
        Array.Copy(buff, nbuff, buff.Length);
        arrayPool.Return(buff);
        buff = nbuff;
    }

    /// <summary>
    /// Creates and returns a string containing the contents of this string builder.
    /// </summary>
    /// <returns></returns>
    public override readonly string ToString()
    {
        return new string(buff, 0, pos);
    }

    public void Append(char c)
    {
        GrowIfNeeded(1);
        buff[pos++] = c;
    }

    public void Append(ReadOnlySpan<char> c)
    {
        GrowIfNeeded(c.Length);
        c.CopyTo(buff.AsSpan(pos));
        pos += c.Length;
    }

    public void Append(string c)
    {
        GrowIfNeeded(c.Length);
        c.CopyTo(0, buff, pos, c.Length);
        pos += c.Length;
    }

    public void AppendLine(ReadOnlySpan<char> c)
    {
        Append(c);
        Append(Environment.NewLine);
    }

    public void AppendLine(string c)
    {
        Append(c);
        Append(Environment.NewLine);
    }

    public void Append(float c)
    {
        Append(c, default);
    }

    public void Append(double c)
    {
        Append(c, default);
    }

    public void Append(nint c)
    {
        Append(c, default);
    }

    public void Append(long c)
    {
        Append(c, default);
    }

    public void Append(int c)
    {
        Append(c, default);
    }

    public void Append(short c)
    {
        Append(c, default);
    }

    public void Append(sbyte c)
    {
        Append(c, default);
    }

    public void Append(ulong c)
    {
        Append(c, default);
    }

    public void Append(uint c)
    {
        Append(c, default);
    }

    public void Append(ushort c)
    {
        Append(c, default);
    }

    public void Append(byte c)
    {
        Append(c, default);
    }

    public void Append(ISpanFormattable c, ReadOnlySpan<char> format, IFormatProvider? formatProvider = null)
    {
        for (int i = 0; i < 16; i++)
        {
            bool res = c.TryFormat(buff.AsSpan(pos), out int written, format, formatProvider);
            if (res)
            {
                pos += written;
                break;
            }
            GrowIfNeeded(buff.Length - pos + 1);
        }
    }

    public readonly void Dispose()
    {
        arrayPool.Return(buff);
    }
}
