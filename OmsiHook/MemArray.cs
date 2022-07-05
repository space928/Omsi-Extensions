using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    /// or whenever <seealso cref="UpdateFromHook"/> is called. Cached arrays 
    /// </remarks>
    /// <typeparam name="T">The type of struct to wrap</typeparam>
    public class MemArray<T> : OmsiObject, IDisposable, IEnumerable<T>, ICollection<T>, IList<T> where T : struct
    {
        private T[] arrayCache;
        private readonly bool cached;

        /// <summary>
        /// Whether or not this <seealso cref="MemArray{T}"/> is cached.
        /// </summary>
        public bool Cached => cached;

        public MemArray() : base() { }

        /// <summary>
        /// Constructs a new MemArray to wrap a native array at a given address.
        /// </summary>
        /// <param name="memory">Instance of the memory manager</param>
        /// <param name="address">Address of the native array to wrap</param>
        /// <param name="cached">Whether or not to copy the contents of the array to a local cache</param>
        internal MemArray(Memory memory, int address, bool cached = true)
        {
            this.cached = cached;
            InitObject(memory, address);
        }

        /// <summary>
        /// Call this method to initialise an OmsiObject if the two-parameter constructor wasn't used.
        /// </summary>
        /// <param name="memory"></param>
        /// <param name="address"></param>
        internal new void InitObject(Memory memory, int address)
        {
            base.InitObject(memory, address);
            UpdateFromHook();
        }

        /// <summary>
        /// Forces the cached contents of the MemArray to resynchronise with the hooked application.
        /// </summary>
        public void UpdateFromHook()
        {
            arrayCache = Memory.ReadMemoryStructArray<T>(Address);
        }

        public int Count => cached ? arrayCache.Length : Memory.ReadMemory<int>(Address - 4);

        public bool IsReadOnly => true;

        public T this[int index] 
        { 
            get => cached ? arrayCache [index] : Memory.ReadMemoryArrayItemSafe<T>(Address, index);
            set
            {
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
        public void Add(T item)
        {
            int arr = Memory.ReadMemory<int>(Address);
            int len = Memory.ReadMemory<int>(arr - 4);
            Memory.WriteMemory(Address, Memory.AllocateArray<T>(++len));
            if(cached)
            {
                // Copy native array from cache
                for(int i = 0; i < Math.Min(len, arrayCache.Length); i++)
                    Memory.WriteMemoryArrayItem(Address, arrayCache[i], i);

                // Update cached array
                Array.Resize(ref arrayCache, len);
                arrayCache[len - 1] = item;
            }
            else
            {
                // Copy native array
                // TODO: Use a raw byte copy for increased efficiency
                for (int i = 0; i < len; i++)
                    Memory.WriteMemoryArrayItem(Address, Memory.ReadMemoryArrayItem<T>(Address, i), i);
            }

            // Add the new item to the native array
            Memory.WriteMemoryArrayItem(Address, item, len-1);
        }

        /// <summary>
        /// Clears the native array but maintains the reference to prevent the GC from destroying it.
        /// </summary>
        public void Clear()
        {
            Memory.WriteMemory(Address, Memory.AllocateArray<T>(0));
            arrayCache = new T[0];
        }

        public bool Contains(T item) => cached ? arrayCache.Contains(item) : Memory.ReadMemoryStructArray<T>(Address).Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (cached)
                arrayCache.CopyTo(array, arrayIndex);
            else
                Memory.ReadMemoryStructArray<T>(Address).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// TODO: Not yet implemented for non-cached arrays.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Not yet implemented for non-cached arrays.</exception>
        public IEnumerator<T> GetEnumerator()
        {
            if(cached)
                return (IEnumerator<T>)arrayCache.GetEnumerator();

            throw new NotImplementedException();
        }

        public bool Remove(T item) => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => arrayCache.GetEnumerator();

        public int IndexOf(T item) => cached ? Array.IndexOf(arrayCache, item) : Array.IndexOf(Memory.ReadMemoryStructArray<T>(Address), item);

        public void Insert(int index, T item) => throw new NotImplementedException();

        public void RemoveAt(int index) => throw new NotImplementedException();

        /// <summary>
        /// Attemps to free the memory allocated to the array if it's no longer referenced by OMSI.
        /// </summary>
        /// <remarks>
        /// For now this just clears the native array and removes all references so that hopefully the GC can clean it up.
        /// </remarks>
        public void Dispose()
        {
            // TODO: Free the old array
            //Memory.Free(Memory.ReadMemory<int>(Address));
            // Remove references from current array. TODO: Does this work? Is this safe?
            Memory.WriteMemory(Memory.ReadMemory<int>(Address) - 8, 0);
            Memory.WriteMemory(Address, Memory.AllocateArray<T>(0, 0));
        }
    }
}
