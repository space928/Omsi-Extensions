using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Wrapper for dynamic arrays of pointers in OMSI's Memory.
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="MemArrayBase{Struct}"/>
    /// </remarks>
    /// <typeparam name="T"><inheritdoc cref="MemArrayBase{Struct}"/></typeparam>
    public class MemArrayPtr<T> : MemArray<T> where T : unmanaged
    {
        public override T[] WrappedArray => cached ? arrayCache : Memory.ReadMemoryStructPtrArray<T>(Address);

        public MemArrayPtr() : base() { }
        internal MemArrayPtr(Memory memory, int address, bool cached = true) : base(memory, address, cached) { }

        public override void UpdateFromHook(int index = -1)
        {
            if (cached)
            {
                if (index >= 0)
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
            var allocTask = Task.WhenAll(Memory.AllocateStructArray<int>(++len), Memory.AllocateStruct(item));
            int narr = allocTask.Result[0];
            int ndata = allocTask.Result[1];
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
            base.Clear();
            if (cached)
                arrayCache = Array.Empty<T>();
        }

        public override bool Remove(T item) => throw new NotImplementedException();

        public override void Insert(int index, T item) => throw new NotImplementedException();

        public override void RemoveAt(int index) => throw new NotImplementedException();
    }
}
