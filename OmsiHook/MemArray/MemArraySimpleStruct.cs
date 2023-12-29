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
    /// Wrapper for dynamic arrays in OMSI's Memory.
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="MemArrayBase{Struct}"/>
    /// </remarks>
    /// <typeparam name="T"><inheritdoc cref="MemArrayBase{Struct}"/></typeparam>
    public class MemArray<T> : MemArray<T, T> where T : unmanaged
    {
        public override T[] WrappedArray => cached ? arrayCache : Memory.ReadMemoryStructArray<T>(Address);

        public MemArray() : base() { }
        internal MemArray(Memory memory, int address, bool cached = true) : base(memory, address, cached) { }

        public override void UpdateFromHook(int index = -1)
        {
            if (cached)
            {
                if (index >= 0)
                    arrayCache[index] = Memory.ReadMemoryArrayItem<T>(Address, index);
                else
                    arrayCache = Memory.ReadMemoryStructArray<T>(Address);
            }
        }

        public override T this[int index]
        {
            get => cached ? arrayCache[index] : Memory.ReadMemoryArrayItemSafe<T>(Address, index);
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
            int narr = Memory.AllocateStructArray<T>(++len).Result;
            Memory.WriteMemory(Address, narr);
            if (cached)
            {
                // Copy native array from cache
                for (int i = 0; i < Math.Min(len, arrayCache.Length); i++)
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
            Memory.WriteMemoryArrayItem(Address, item, len - 1);
        }

        /// <summary>
        /// Clears the native array but maintains the reference to prevent the GC from destroying it.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            if (cached)
                arrayCache = Array.Empty<T>();
        }

        public override bool Remove(T item) => throw new NotImplementedException();

        public override void Insert(int index, T item) => throw new NotImplementedException();

        public override void RemoveAt(int index) => throw new NotImplementedException();
    }
}
