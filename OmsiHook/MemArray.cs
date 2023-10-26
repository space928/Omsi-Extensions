using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Wrapper for Arrays / Lists in OMSI's Memory.
    /// </summary>
    /// <remarks>
    /// This is a heavyweight wrapper for native arrays that provides methods for reading and writing to arrays as well as 
    /// helping with memory management. For fast, low-level access, use the methods in the <seealso cref="Memory"/> class. <para/>
    /// For better performance in c# the contents of the wrapped array can be copied to managed memory when constructed
    /// or whenever <seealso cref="UpdateFromHook"/> is called. <para/>
    /// Cached arrays are generally faster when accessed or searched frequently by C#, but they are slower to update and 
    /// the user is responsible for ensuring that they are synchronised with the native array it wraps.
    /// </remarks>
    /// <typeparam name="Struct">The type of struct to wrap</typeparam>
    /// <typeparam name="InternalStruct">Internal struct type to marshal <c>Struct</c> from</typeparam>
    public class MemArray<InternalStruct, Struct> : MemArrayBase<Struct>
        where InternalStruct : unmanaged
        where Struct : struct
    {

        /// <summary>
        /// Gets the current contents of the wrapped array. On non-cached arrays this is slow.
        /// </summary>
        public override Struct[] WrappedArray => cached ? arrayCache
            : Memory.MarshalStructs<Struct, InternalStruct>(Memory.ReadMemoryStructArray<InternalStruct>(Address));

        public MemArray() : base() { }

        internal MemArray(Memory memory, int address, bool cached = true) : base(memory, address, cached) { }

        public override void UpdateFromHook(int index = -1)
        {
            if (cached)
            {
                if (index < 0)
                    arrayCache = Memory.MarshalStructs<Struct, InternalStruct>(Memory.ReadMemoryStructArray<InternalStruct>(Address));
                else
                    arrayCache[index] = Memory.MarshalStruct<Struct, InternalStruct>(Memory.ReadMemoryArrayItem<InternalStruct>(Address, index));
            }
        }

        public override Struct this[int index]
        {
            get => cached ? arrayCache[index]
                : Memory.MarshalStruct<Struct, InternalStruct>(
                    Memory.ReadMemoryArrayItemSafe<InternalStruct>(Address, index));
            set
            {
                if (cached)
                    arrayCache[index] = value;
                Memory.WriteMemoryArrayItemSafe(Address, Memory.UnMarshalStruct<InternalStruct, Struct>(value), index);
            }
        }

        /// <summary>
        /// TODO: Implement efficient enumerator for non-cached arrays.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<Struct> GetEnumerator() => ((IEnumerable<Struct>)WrappedArray).GetEnumerator();

        /// <summary>
        /// Adds an item to the native array. This is slow and might cause memory leaks because it reallocates the whole array.
        /// </summary>
        /// <remarks>
        /// Since this method doesn't call <seealso cref="UpdateFromHook"/>, if the native array is out of sync 
        /// with the cached array, then data may be lost when adding new items.
        /// In the following case though, it should usually be safe (as long as the native array isn't updated by Omsi while this runs):
        /// <code lang="c">
        /// memArray.UpdateFromHook();
        /// foreach(int item in localObjects)
        ///     memArray.Add(item);
        /// </code>
        /// </remarks>
        /// <param name="item">The item to add to the native array.</param>
        public override void Add(Struct item)
        {
            int arr = Memory.ReadMemory<int>(Address);
            int len = Memory.ReadMemory<int>(arr - 4);
            int narr = Memory.AllocateStructArray<InternalStruct>(++len);
            Memory.WriteMemory(Address, narr);
            if (cached)
            {
                // Copy native array from cache
                for (int i = 0; i < Math.Min(len, arrayCache.Length); i++)
                    Memory.WriteMemoryArrayItem(narr, Memory.UnMarshalStruct<InternalStruct, Struct>(arrayCache[i]), i);

                // Update cached array
                Array.Resize(ref arrayCache, len);
                arrayCache[len - 1] = item;
            }
            else
            {
                // Copy native array
                Memory.CopyMemory(arr, narr, (len-1) * Marshal.SizeOf<InternalStruct>());
            }

            // Add the new item to the native array
            Memory.WriteMemoryArrayItem(Address, Memory.UnMarshalStruct<InternalStruct, Struct>(item), len - 1);
        }

        public override void Clear()
        {
            Memory.WriteMemory(Address, Memory.AllocateStructArray<InternalStruct>(0));
            if(cached)
                arrayCache = Array.Empty<Struct>();
        }

        public override bool Remove(Struct item) => throw new NotImplementedException();

        public override void Insert(int index, Struct item) => throw new NotImplementedException();

        public override void RemoveAt(int index) => throw new NotImplementedException();

        public override int IndexOf(Struct item) => Array.IndexOf(WrappedArray, item);

        public override bool Contains(Struct item) => WrappedArray.Contains(item);

        public override void CopyTo(Struct[] array, int arrayIndex) => WrappedArray.CopyTo(array, arrayIndex);

        public override void Dispose()
        {
            // TODO: Free the old array
            //Memory.Free(Memory.ReadMemory<int>(Address));
            // Remove references from current array. TODO: Does this work? Is this safe?
            Memory.WriteMemory(Memory.ReadMemory<int>(Address) - 8, 0);
            Memory.WriteMemory(Address, Memory.AllocateStructArray<InternalStruct>(0, 0));

            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Wrapper for Arrays / Lists in OMSI's Memory.
    /// </summary>
    /// <remarks>
    /// This is a heavyweight wrapper for native arrays that provides methods for reading and writing to arrays as well as 
    /// helping with memory management. For fast, low-level access, use the methods in the <seealso cref="Memory"/> class. <para/>
    /// For better performance in c# the contents of the wrapped array can be copied to managed memory when constructed
    /// or whenever <seealso cref="UpdateFromHook"/> is called. <para/>
    /// Cached arrays are generally faster when accessed or searched frequently by C#, but they are slower to update and 
    /// the user is responsible for ensuring that they are synchronised with the native array it wraps.
    /// </remarks>
    /// <typeparam name="T">The type of struct to wrap.</typeparam>
    public class MemArray<T> : MemArray<T, T> where T : unmanaged
    {
        public override T[] WrappedArray => cached ? arrayCache : Memory.ReadMemoryStructArray<T>(Address);

        public MemArray() : base() { }
        internal MemArray(Memory memory, int address, bool cached = true) : base(memory, address, cached) { }

        public override void UpdateFromHook(int index = -1)
        {
            if (cached)
            {
                if(index >= 0)
                    arrayCache[index] = Memory.ReadMemoryArrayItem<T>(Address, index);
                else
                    arrayCache = Memory.ReadMemoryStructArray<T>(Address);
            }
        }

        public override T this[int index] 
        { 
            get => cached ? arrayCache [index] : Memory.ReadMemoryArrayItemSafe<T>(Address, index);
            set
            {
                if (cached)
                    arrayCache[index] = value;
                Memory.WriteMemoryArrayItemSafe(Address, value, index);
            }
        }

        /// <summary>
        /// Adds an item to the native array. This is slow and might cause memory leaks because it reallocates the whole array.
        /// </summary>
        /// <remarks>
        /// Since this method doesn't call <seealso cref="UpdateFromHook"/>, if the native array is out of sync 
        /// with the cached array, then data may be lost when adding new items.
        /// In the following case though, it should usually be safe (as long as the native array isn't updated by Omsi while this runs):
        /// <code lang="c">
        /// memArray.UpdateFromHook();
        /// foreach(int item in localObjects)
        ///     memArray.Add(item);
        /// </code>
        /// </remarks>
        /// <param name="item">The item to add to the native array.</param>
        public override void Add(T item)
        {
            int arr = Memory.ReadMemory<int>(Address);
            int len = Memory.ReadMemory<int>(arr - 4);
            int narr = Memory.AllocateStructArray<T>(++len);
            Memory.WriteMemory(Address, narr);
            if(cached)
            {
                // Copy native array from cache
                for(int i = 0; i < Math.Min(len, arrayCache.Length); i++)
                    Memory.WriteMemoryArrayItem(narr, arrayCache[i], i);

                // Update cached array
                Array.Resize(ref arrayCache, len);
                arrayCache[len - 1] = item;
            }
            else
            {
                // Copy native array
                Memory.CopyMemory(arr, narr, (len - 1) * Marshal.SizeOf<T>());
            }

            // Add the new item to the native array
            Memory.WriteMemoryArrayItem(Address, item, len-1);
        }

        /// <summary>
        /// Clears the native array but maintains the reference to prevent the GC from destroying it.
        /// </summary>
        public override void Clear()
        {
            Memory.WriteMemory(Address, Memory.AllocateStructArray<T>(0));
            if (cached)
                arrayCache = Array.Empty<T>();
        }

        public override bool Remove(T item) => throw new NotImplementedException();

        public override void Insert(int index, T item) => throw new NotImplementedException();

        public override void RemoveAt(int index) => throw new NotImplementedException();

        /// <summary>
        /// Attemps to free the memory allocated to the array if it's no longer referenced by OMSI.
        /// </summary>
        /// <remarks>
        /// For now this just clears the native array and removes all references so that hopefully the GC can clean it up.
        /// </remarks>
        public override void Dispose()
        {
            // TODO: Free the old array
            //Memory.Free(Memory.ReadMemory<int>(Address));
            // Remove references from current array. TODO: Does this work? Is this safe?
            Memory.WriteMemory(Memory.ReadMemory<int>(Address) - 8, 0);
            Memory.WriteMemory(Address, Memory.AllocateStructArray<T>(0, 0));

            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Wrapper for Arrays / Lists in OMSI's Memory.
    /// </summary>
    /// <remarks>
    /// This is a heavyweight wrapper for native arrays that provides methods for reading and writing to arrays as well as 
    /// helping with memory management. For fast, low-level access, use the methods in the <seealso cref="Memory"/> class. <para/>
    /// For better performance in c# the contents of the wrapped array can be copied to managed memory when constructed
    /// or whenever <seealso cref="UpdateFromHook"/> is called. <para/>
    /// Cached arrays are generally faster when accessed or searched frequently by C#, but they are slower to update and 
    /// the user is responsible for ensuring that they are synchronised with the native array it wraps.
    /// </remarks>
    /// <typeparam name="T">The type of struct to wrap.</typeparam>
    public class MemArrayPtr<T> : MemArray<T> where T : unmanaged
    {
        public override T[] WrappedArray => cached ? arrayCache : Memory.ReadMemoryStructPtrArray<T>(Address);

        public MemArrayPtr() : base() { }
        internal MemArrayPtr(Memory memory, int address, bool cached = true) : base(memory, address, cached) { }

        public override void UpdateFromHook(int index = -1)
        {
            if (cached)
            {
                if(index >= 0)
                    arrayCache[index] = Memory.ReadMemoryArrayItem<T>(Address, index, true);
                else
                    arrayCache = Memory.ReadMemoryStructPtrArray<T>(Address);
            }
        }

        public override T this[int index]
        {
            get => cached ? arrayCache[index] : Memory.ReadMemoryArrayItemSafe<T>(Address, index, true);
            set
            {
                if (cached)
                    arrayCache[index] = value;
                Memory.WriteMemoryArrayItemSafe(Address, value, index, true);
            }
        }

        public override void Add(T item)
        {
            int arr = Memory.ReadMemory<int>(Address);
            int len = Memory.ReadMemory<int>(arr - 4);
            int narr = Memory.AllocateStructArray<int>(++len);
            int ndata = Memory.AllocateStruct(item);
            Memory.WriteMemory(Address, narr);
            if (cached)
            {
                // Copy native array
                Memory.CopyMemory(arr, narr, (len - 1) * 4);

                // Update cached array
                Array.Resize(ref arrayCache, len);
                arrayCache[len - 1] = item;
            }
            else
            {
                // Copy native array
                Memory.CopyMemory(arr, narr, (len - 1) * 4);
            }

            // Add the new item to the native array
            Memory.WriteMemoryArrayItem(Address, ndata, len - 1, false);
        }

        /// <summary>
        /// Clears the native array but maintains the reference to prevent the GC from destroying it.
        /// Note that this does not free or dereference the items pointed to by the array elements.
        /// </summary>
        public override void Clear()
        {
            Memory.WriteMemory(Address, Memory.AllocateStructArray<T>(0));
            if (cached)
                arrayCache = Array.Empty<T>();
        }

        public override bool Remove(T item) => throw new NotImplementedException();

        public override void Insert(int index, T item) => throw new NotImplementedException();

        public override void RemoveAt(int index) => throw new NotImplementedException();

        /// <summary>
        /// Attemps to free the memory allocated to the array if it's no longer referenced by OMSI.
        /// </summary>
        /// <remarks>
        /// For now this just clears the native array and removes all references so that hopefully the GC can clean it up.
        /// </remarks>
        public override void Dispose()
        {
            // TODO: Free the old array
            //Memory.Free(Memory.ReadMemory<int>(Address));
            // Remove references from current array. TODO: Does this work? Is this safe?
            Memory.WriteMemory(Memory.ReadMemory<int>(Address) - 8, 0);
            Memory.WriteMemory(Address, Memory.AllocateStructArray<T>(0, 0));

            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Wrapper for Arrays / Lists in OMSI's Memory.
    /// </summary>
    /// <remarks>
    /// This is a heavyweight wrapper for native arrays that provides methods for reading and writing to arrays as well as 
    /// helping with memory management. For fast, low-level access, use the methods in the <seealso cref="Memory"/> class. <para/>
    /// For better performance in c# the contents of the wrapped array can be copied to managed memory when constructed
    /// or whenever <seealso cref="UpdateFromHook"/> is called. <para/>
    /// Cached arrays are generally faster when accessed or searched frequently by C#, but they are slower to update and 
    /// the user is responsible for ensuring that they are synchronised with the native array it wraps.
    /// </remarks>
    public class MemArrayString : MemArrayBase<string>
    {
        internal readonly bool wide;

        public override string this[int index] 
        { 
            get => cached ? arrayCache[index] : Memory.ReadMemoryArrayItemStringSafe(Address, index, wide); 
            set 
            {
                if (cached)
                    arrayCache[index] = value;
                Memory.WriteMemoryArrayItemSafe(Address, Memory.AllocateString(value), index);
            }
        }

        public override string[] WrappedArray => cached ? arrayCache
            : Memory.ReadMemoryStringArray(Address, wide);

        /// <summary>
        /// Whether or not the string is in UTF-16.
        /// </summary>
        public bool Wide => wide;

        public MemArrayString() : base() { }
        /// <summary>
        /// Constructs a new MemArray to wrap a native array at a given address.
        /// </summary>
        /// <param name="memory">Instance of the memory manager</param>
        /// <param name="address">Address of the native array to wrap</param>
        /// <param name="wide">Whether or not the string is in UTF-16</param>
        /// <param name="cached">Whether or not to copy the contents of the array to a local cache</param>
        internal MemArrayString(Memory memory, int address, bool wide = false, bool cached = true) : base(memory, address, cached)
        {
            this.wide = wide;
        }

        public override void UpdateFromHook(int index = -1)
        {
            if (cached)
            {
                if(index >= 0)
                    arrayCache[index] = Memory.ReadMemoryArrayItemString(Address, index, wide);
                else
                    arrayCache = Memory.ReadMemoryStringArray(Address, wide);
            }
        }

        /// <summary>
        /// Adds an item to the native array. This is slow and might cause memory leaks because it reallocates the whole array.
        /// </summary>
        /// <remarks>
        /// This method is probably slower (and much more memory intensive) on cached arrays. <para/>
        /// Since this method doesn't call <seealso cref="UpdateFromHook"/>, if the native array is out of sync 
        /// with the cached array, then data may be lost when adding new items.
        /// In the following case though, it should usually be safe (as long as the native array isn't updated by Omsi while this runs):
        /// <code lang="c">
        /// memArray.UpdateFromHook();
        /// foreach(int item in localObjects)
        ///     memArray.Add(item);
        /// </code>
        /// </remarks>
        /// <param name="item">The item to add to the native array.</param>
        public override void Add(string item)
        {
            int arr = Memory.ReadMemory<int>(Address);
            int len = Memory.ReadMemory<int>(arr - 4);
            int narr = Memory.AllocateStructArray<int>(++len);
            Memory.WriteMemory(Address, narr);
            if (cached)
            {
                // Copy native array from cache
                for (int i = 0; i < Math.Min(len, arrayCache.Length); i++)
                    Memory.WriteMemoryArrayItem(narr, Memory.AllocateString(arrayCache[i]), i);

                // Update cached array
                Array.Resize(ref arrayCache, len);
                arrayCache[len - 1] = item;
            }
            else
            {
                // Copy native array
                Memory.CopyMemory(arr, narr, (len - 1) * Marshal.SizeOf<int>());
            }

            // Add the new item to the native array
            Memory.WriteMemoryArrayItem(Address, Memory.AllocateString(item), len - 1);
        }

        public override void Clear()
        {
            Memory.WriteMemory(Address, Memory.AllocateStructArray<int>(0));
            if (cached)
                arrayCache = Array.Empty<string>();
        }

        public override bool Contains(string item) => WrappedArray.Contains(item);

        public override void CopyTo(string[] array, int arrayIndex) => WrappedArray.CopyTo(array, arrayIndex);

        //TODO: Implement efficient enumerator for non-cached arrays.
        public override IEnumerator<string> GetEnumerator() => (IEnumerator<string>)WrappedArray.GetEnumerator();

        public override int IndexOf(string item) => Array.IndexOf(WrappedArray, item);

        public override void Insert(int index, string item) => throw new NotImplementedException();

        public override bool Remove(string item) => throw new NotImplementedException();

        public override void RemoveAt(int index) => throw new NotImplementedException();

        public override void Dispose()
        {
            // TODO: Free the old array
            //Memory.Free(Memory.ReadMemory<int>(Address));
            // Remove references from current array. TODO: Does this work? Is this safe?
            Memory.WriteMemory(Memory.ReadMemory<int>(Address) - 8, 0);
            Memory.WriteMemory(Address, Memory.AllocateStructArray<int>(0, 0));

            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Wrapper for Arrays / Lists in OMSI's Memory with automatic index caching.
    /// </summary>
    /// <remarks>
    /// This type of MemArray relies on it being cached and as such no option exists to use it uncached.
    /// This is a heavyweight wrapper for native arrays that provides methods for reading and writing to arrays as well as 
    /// helping with memory management. For fast, low-level access, use the methods in the <seealso cref="Memory"/> class. <para/>
    /// For better performance in c# the contents of the wrapped array can be copied to managed memory when constructed
    /// or whenever <seealso cref="UpdateFromHook"/> is called. <para/>
    /// Cached arrays are generally faster when accessed or searched frequently by C#, but they are slower to update and 
    /// the user is responsible for ensuring that they are synchronised with the native array it wraps.
    /// </remarks>
    public class MemArrayStringDict : MemArrayString
    { 
        private Dictionary<string, int> indexDictionary = new();

        public Dictionary<string, int> IndexDictionary => indexDictionary;

        internal MemArrayStringDict(Memory memory, int address, bool wide = false) : base(memory, address, wide, true) { }
        public MemArrayStringDict() : base() { }

        /// <summary>
        /// Gets the index of the string in the native array from it's value.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int this[string item] => indexDictionary[item];

        public override bool Cached { get => cached; }

        public override void UpdateFromHook(int index = -1)
        {
            if (index >= 0)
            {
                string prevValue = arrayCache[index];
                base.UpdateFromHook(index);
                if (prevValue != arrayCache[index])
                {
                    indexDictionary.Remove(prevValue);
                    indexDictionary.Add(arrayCache[index], index);
                }
            }
            else
            {
                base.UpdateFromHook(index);
                indexDictionary.Clear();
                for (int i = 0; i < Count; i++)
                {
                    if (arrayCache[i] != null)
                        indexDictionary.TryAdd(arrayCache[i], i);
                }
            }
        }

        public override bool Contains(string item) => indexDictionary.ContainsKey(item);
        public override int IndexOf(string item) => indexDictionary[item];
        public override bool Remove(string item)
        {
            if (indexDictionary.ContainsKey(item))
                base.RemoveAt(indexDictionary[item]);
            else
                return false;
            return true;
        }
    }
}
