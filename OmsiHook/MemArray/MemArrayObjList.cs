using System;
using System.Collections.Generic;
using System.Linq;
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
public class MemArrayList<T> : MemArrayBase<T> where T : OmsiObject, new()
{
    private T[] ReadList()
    {
        uint arr = Memory.ReadMemory<uint>(Address);
        if (arr == 0)
            return Array.Empty<T>();
        uint len = Memory.ReadMemory<uint>(arr + 8);
        uint arrayData = Memory.ReadMemory<uint>(arr + 4);
        T[] ret = new T[len];
        for (uint i = 0; i < len; i++)
        {
            var objAddr = Memory.ReadMemory<uint>(arrayData + i * 4);
            if (objAddr == 0)
            {
                ret[i] = null;
                continue;
            }

            var n = new T();
            n.InitObject(Memory, (int)objAddr);
            ret[i] = n;
        }

        return ret;
    }

    private T ReadListItem(int index)
    {
        int arr = Memory.ReadMemory<int>(Address);
        if (arr == 0)
            return null;

        int len = Memory.ReadMemory<int>(arr + 8);
        int arrayData = Memory.ReadMemory<int>(arr + 4);

        if (index >= len || index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));

        var objAddr = Memory.ReadMemory<int>(arrayData + index * 4);
        if (objAddr == 0)
            return null;

        var n = new T();
        n.InitObject(Memory, objAddr);

        return n;
    }

    /// <summary>
    /// Gets the current contents of the wrapped array. On non-cached arrays this is slow.
    /// </summary>
    public override T[] WrappedArray => cached ? arrayCache : ReadList();

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

    public override T this[int index]
    {
        get => cached ? arrayCache[index] : ReadListItem(index);
        set => throw new NotSupportedException();
    }

    /// <summary>
    /// TODO: Implement efficient enumerator for non-cached arrays.
    /// https://github.com/space928/Omsi-Extensions/issues/110
    /// </summary>
    /// <returns></returns>
    public override IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)WrappedArray).GetEnumerator();

    public override void Add(T item)
    {
        throw new NotSupportedException();
    }

    public override void Clear()
    {
        base.Clear();
        if (cached)
            arrayCache = Array.Empty<T>();
    }

    public override bool Remove(T item) => throw new NotImplementedException();

    public override void Insert(int index, T item) => throw new NotImplementedException();

    public override void RemoveAt(int index) => throw new NotImplementedException();

    public override int IndexOf(T item) => Array.IndexOf(WrappedArray, item);

    public override bool Contains(T item) => WrappedArray.Contains(item);

    public override void CopyTo(T[] array, int arrayIndex) => WrappedArray.CopyTo(array, arrayIndex);
}

/// <summary>
/// Wrapper for OLists in OMSI's Memory.
/// </summary>
/// <remarks>
/// <inheritdoc cref="MemArrayBase{Struct}"/>
/// </remarks>
/// <typeparam name="T"><inheritdoc cref="MemArrayBase{Struct}"/></typeparam>
public class MemArrayOList<T> : MemArrayBase<T> where T : OmsiObject, new()
{
    public int IntCount => Memory.ReadMemory<int>(Address + 0x8);
    //public int Count => Memory.ReadMemory<int>(Address + 0xc);
    public int MaxStepCount => Memory.ReadMemory<byte>(Address + 0x10);

    private T[] ReadList()
    {
        uint arr = Memory.ReadMemory<uint>(Address);
        if (arr == 0)
            return Array.Empty<T>();
        uint len = Memory.ReadMemory<uint>(arr + 0xc);
        uint arrayData = Memory.ReadMemory<uint>(arr + 4);
        T[] ret = new T[len];
        for (uint i = 0; i < len; i++)
        {
            var objAddr = Memory.ReadMemory<uint>(arrayData + i * 4);
            if (objAddr == 0)
            {
                ret[i] = null;
                continue;
            }

            var n = new T();
            n.InitObject(Memory, (int)objAddr);
            ret[i] = n;
        }

        return ret;
    }

    private T ReadListItem(int index)
    {
        int arr = Memory.ReadMemory<int>(Address);
        if (arr == 0)
            return null;

        int len = Memory.ReadMemory<int>(arr + 0xc);
        int arrayData = Memory.ReadMemory<int>(arr + 4);

        if (index >= len || index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));

        var objAddr = Memory.ReadMemory<int>(arrayData + index * 4);
        if (objAddr == 0)
            return null;

        var n = new T();
        n.InitObject(Memory, objAddr);

        return n;
    }

    /// <summary>
    /// Gets the current contents of the wrapped array. On non-cached arrays this is slow.
    /// </summary>
    public override T[] WrappedArray => cached ? arrayCache : ReadList();

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

    public override T this[int index]
    {
        get => cached ? arrayCache[index] : ReadListItem(index);
        set => throw new NotSupportedException();
    }

    /// <summary>
    /// TODO: Implement efficient enumerator for non-cached arrays.
    /// https://github.com/space928/Omsi-Extensions/issues/110
    /// </summary>
    /// <returns></returns>
    public override IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)WrappedArray).GetEnumerator();

    public override void Add(T item)
    {
        throw new NotSupportedException();
    }

    public override void Clear()
    {
        base.Clear();
        if (cached)
            arrayCache = Array.Empty<T>();
    }

    public override bool Remove(T item) => throw new NotImplementedException();

    public override void Insert(int index, T item) => throw new NotImplementedException();

    public override void RemoveAt(int index) => throw new NotImplementedException();

    public override int IndexOf(T item) => Array.IndexOf(WrappedArray, item);

    public override bool Contains(T item) => WrappedArray.Contains(item);

    public override void CopyTo(T[] array, int arrayIndex) => WrappedArray.CopyTo(array, arrayIndex);
}
