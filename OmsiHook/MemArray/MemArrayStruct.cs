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
    /// <inheritdoc cref="MemArrayBase{Struct}"/>
    /// </remarks>
    /// <typeparam name="Struct"><inheritdoc cref="MemArrayBase{Struct}"/></typeparam>
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
            int narr = Memory.AllocateStructArray<InternalStruct>(++len).Result;
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
            base.Clear();
            if(cached)
                arrayCache = Array.Empty<Struct>();
        }

        public override bool Remove(Struct item) => throw new NotImplementedException();

        public override void Insert(int index, Struct item) => throw new NotImplementedException();

        public override void RemoveAt(int index) => throw new NotImplementedException();

        public override int IndexOf(Struct item) => Array.IndexOf(WrappedArray, item);

        public override bool Contains(Struct item) => WrappedArray.Contains(item);

        public override void CopyTo(Struct[] array, int arrayIndex) => WrappedArray.CopyTo(array, arrayIndex);
    }
}
