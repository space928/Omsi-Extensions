using System;
using System.Collections;
using System.Collections.Generic;

namespace OmsiHook
{
    public abstract class MemArrayBase<Struct> : OmsiObject, IDisposable, IEnumerable<Struct>, ICollection<Struct>, IList<Struct>
    {
        internal readonly bool cached;
        internal Struct[] arrayCache;

        public abstract Struct this[int index] { get; set; }

        /// <summary>
        /// Whether or not this <seealso cref="MemArray{T}"/> is cached.
        /// </summary>
        public bool Cached => cached;

        public abstract Struct[] WrappedArray { get; }

        public int Count => cached ? arrayCache.Length : Memory.ReadMemory<int>(Address - 4);

        public bool IsReadOnly => false;

        public MemArrayBase() : base() { }

        /// <summary>
        /// Constructs a new MemArray to wrap a native array at a given address.
        /// </summary>
        /// <param name="memory">Instance of the memory manager</param>
        /// <param name="address">Address of the native array to wrap</param>
        /// <param name="cached">Whether or not to copy the contents of the array to a local cache</param>
        internal MemArrayBase(Memory memory, int address, bool cached = true)
        {
            this.cached = cached;
            InitObject(memory, address);
        }

        public abstract void Add(Struct item);
        public abstract void Clear();
        public abstract bool Contains(Struct item);
        public abstract void CopyTo(Struct[] array, int arrayIndex);
        public abstract void Dispose();
        public abstract IEnumerator<Struct> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => WrappedArray.GetEnumerator();
        public abstract int IndexOf(Struct item);
        public abstract void Insert(int index, Struct item);
        public abstract bool Remove(Struct item);
        public abstract void RemoveAt(int index);
        public abstract void UpdateFromHook();

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
    }
}