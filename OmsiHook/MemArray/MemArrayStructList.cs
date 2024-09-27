using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook;

/// <summary>
/// Wrapper for Lists in OMSI's Memory.
/// </summary>
/// <remarks>
/// <inheritdoc cref="MemArrayBase{Struct}"/>
/// </remarks>
/// <typeparam name="T"><inheritdoc cref="MemArrayBase{Struct}"/></typeparam>
/// <typeparam name="InternalStruct">Internal struct type to marshal <c>Struct</c> from</typeparam>
public class MemArrayList<InternalStruct, Struct> : MemArrayBase<Struct>
    where InternalStruct : unmanaged
    where Struct : struct
{
    private Struct[] ReadList()
    {
        uint arr = Memory.ReadMemory<uint>(Address);
        if (arr == 0)
            return Array.Empty<Struct>();
        uint len = Memory.ReadMemory<uint>(arr + 8);
        uint arrayData = Memory.ReadMemory<uint>(arr + 4);
        Struct[] ret = new Struct[len];
        for (uint i = 0; i < len; i++)
        {
            var objAddr = Memory.ReadMemory<uint>(arrayData + i * 4);
            if (objAddr == 0)
            {
                ret[i] = default;
                continue;
            }

            var n = Memory.MarshalStruct<Struct, InternalStruct>(Memory.ReadMemory<InternalStruct>(objAddr));
            ret[i] = n;
        }

        return ret;
    }

    private Struct ReadListItem(int index)
    {
        int arr = Memory.ReadMemory<int>(Address);
        if (arr == 0)
            return default;

        int len = Memory.ReadMemory<int>(arr + 8);
        int arrayData = Memory.ReadMemory<int>(arr + 4);

        if (index >= len || index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));

        var objAddr = Memory.ReadMemory<int>(arrayData + index * 4);
        if (objAddr == 0)
            return default;

        var n = Memory.MarshalStruct<Struct, InternalStruct>(Memory.ReadMemory<InternalStruct>(objAddr));

        return n;
    }

    /// <summary>
    /// Gets the current contents of the wrapped array. On non-cached arrays this is slow.
    /// </summary>
    public override Struct[] WrappedArray => cached ? arrayCache : ReadList();

    public MemArrayList() : base() { }

    internal MemArrayList(Memory memory, int address, bool cached = true) : base(memory, address, cached) { }

    public override void UpdateFromHook(int index = -1)
    {
        if (cached)
        {
            if (index < 0)
                arrayCache = ReadList();
            else
                arrayCache[index] = ReadListItem(index);
        }
    }

    public override Struct this[int index]
    {
        get => cached ? arrayCache[index] : ReadListItem(index);
        set => throw new NotSupportedException();
    }

    /// <summary>
    /// TODO: Implement efficient enumerator for non-cached arrays.
    /// https://github.com/space928/Omsi-Extensions/issues/110
    /// </summary>
    /// <returns></returns>
    public override IEnumerator<Struct> GetEnumerator() => ((IEnumerable<Struct>)WrappedArray).GetEnumerator();

    public override void Add(Struct item)
    {
        throw new NotSupportedException();
    }

    public override void Clear()
    {
        base.Clear();
        if (cached)
            arrayCache = Array.Empty<Struct>();
    }

    public override bool Remove(Struct item) => throw new NotImplementedException();

    public override void Insert(int index, Struct item) => throw new NotImplementedException();

    public override void RemoveAt(int index) => throw new NotImplementedException();

    public override int IndexOf(Struct item) => Array.IndexOf(WrappedArray, item);

    public override bool Contains(Struct item) => WrappedArray.Contains(item);

    public override void CopyTo(Struct[] array, int arrayIndex) => WrappedArray.CopyTo(array, arrayIndex);
}

/// <summary>
/// Wrapper for Lists in OMSI's Memory.
/// </summary>
/// <remarks>
/// <inheritdoc cref="MemArrayBase{Struct}"/>
/// </remarks>
/// <typeparam name="T"><inheritdoc cref="MemArrayBase{Struct}"/></typeparam>
/// <typeparam name="InternalStruct">Internal struct type to marshal <c>Struct</c> from</typeparam>
public class MemArrayOList<InternalStruct, Struct> : MemArrayBase<Struct>
    where InternalStruct : unmanaged
    where Struct : struct
{
    private Struct[] ReadList()
    {
        uint arr = Memory.ReadMemory<uint>(Address);
        if (arr == 0)
            return Array.Empty<Struct>();
        uint len = Memory.ReadMemory<uint>(arr + 0xc);
        uint arrayData = Memory.ReadMemory<uint>(arr + 4);
        Struct[] ret = new Struct[len];
        for (uint i = 0; i < len; i++)
        {
            var objAddr = arrayData + i * (uint)Unsafe.SizeOf<InternalStruct>();
            if (objAddr == 0)
            {
                ret[i] = default;
                continue;
            }

            var n = Memory.MarshalStruct<Struct, InternalStruct>(Memory.ReadMemory<InternalStruct>(objAddr));
            ret[i] = n;
        }

        return ret;
    }

    private Struct ReadListItem(int index)
    {
        int arr = Memory.ReadMemory<int>(Address);
        if (arr == 0)
            return default;

        int len = Memory.ReadMemory<int>(arr + 0xc);
        int arrayData = Memory.ReadMemory<int>(arr + 4);

        if (index >= len || index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));

        var objAddr = arrayData + index * Unsafe.SizeOf<InternalStruct>();
        if (objAddr == 0)
            return default;

        var n = Memory.MarshalStruct<Struct, InternalStruct>(Memory.ReadMemory<InternalStruct>(objAddr));

        return n;
    }

    /// <summary>
    /// Gets the current contents of the wrapped array. On non-cached arrays this is slow.
    /// </summary>
    public override Struct[] WrappedArray => cached ? arrayCache : ReadList();

    public MemArrayOList() : base() { }

    internal MemArrayOList(Memory memory, int address, bool cached = true) : base(memory, address, cached) { }

    public override void UpdateFromHook(int index = -1)
    {
        if (cached)
        {
            if (index < 0)
                arrayCache = ReadList();
            else
                arrayCache[index] = ReadListItem(index);
        }
    }

    public override Struct this[int index]
    {
        get => cached ? arrayCache[index] : ReadListItem(index);
        set => throw new NotSupportedException();
    }

    /// <summary>
    /// TODO: Implement efficient enumerator for non-cached arrays.
    /// https://github.com/space928/Omsi-Extensions/issues/110
    /// </summary>
    /// <returns></returns>
    public override IEnumerator<Struct> GetEnumerator() => ((IEnumerable<Struct>)WrappedArray).GetEnumerator();

    public override void Add(Struct item)
    {
        throw new NotSupportedException();
    }

    public override void Clear()
    {
        base.Clear();
        if (cached)
            arrayCache = Array.Empty<Struct>();
    }

    public override bool Remove(Struct item) => throw new NotImplementedException();

    public override void Insert(int index, Struct item) => throw new NotImplementedException();

    public override void RemoveAt(int index) => throw new NotImplementedException();

    public override int IndexOf(Struct item) => Array.IndexOf(WrappedArray, item);

    public override bool Contains(Struct item) => WrappedArray.Contains(item);

    public override void CopyTo(Struct[] array, int arrayIndex) => WrappedArray.CopyTo(array, arrayIndex);
}
