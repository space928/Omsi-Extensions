using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Wrapper for arrays of strings in OMSI's memmory.
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="MemArrayBase{Struct}"/>
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
                Memory.WriteMemoryArrayItemSafe(Address, Memory.AllocateString(value, wide).Result, index);
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
                if (index >= 0)
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
            int narr = Memory.AllocateStructArray<int>(++len).Result;
            Memory.WriteMemory(Address, narr);
            if (cached)
            {
                // Copy native array from cache, this will never be faster...
                /*Span<int> allocatedStrings = stackalloc int[Math.Min(len, arrayCache.Length)];
                Task.WhenAll();
                for (int i = 0; i < allocatedStrings.Length; i++)
                    allocatedStrings[i] = Memory.AllocateString(arrayCache[i]);
                for (int i = 0; i < Math.Min(len, arrayCache.Length); i++)
                    Memory.WriteMemoryArrayItem(narr, allocatedStrings[i], i);*/
                // Copy native array
                Memory.CopyMemory(arr, narr, (len - 1) * Marshal.SizeOf<int>());

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
            Memory.WriteMemoryArrayItem(Address, Memory.AllocateString(item).Result, len - 1);
        }

        public override void Clear()
        {
            base.Clear();
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
    }
}
