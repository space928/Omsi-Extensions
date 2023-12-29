using System;
using System.Collections;
using System.Collections.Generic;

namespace OmsiHook
{
    /// <summary>
    /// Base class for wrappers for Arrays / Lists in OMSI's Memory.
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
    public abstract class MemArrayBase<Struct> : OmsiObject, IDisposable, IEnumerable<Struct>, ICollection<Struct>, IList<Struct>
    {
        internal bool cached;
        internal Struct[] arrayCache;

        public abstract Struct this[int index] { get; set; }

        /// <summary>
        /// Whether or not this <seealso cref="MemArray{T}"/> is cached.
        /// </summary>
        public virtual bool Cached { get => cached; set { cached = value; UpdateFromHook(); } }

        public abstract Struct[] WrappedArray { get; }

        public int Count
        {
            get
            {
                if (cached)
                    return arrayCache?.Length ?? 0;
                else
                {
                    int start = Memory.ReadMemory<int>(Address);
                    if (start == 0)
                        return 0;
                        //throw new NullReferenceException();
                    return Memory.ReadMemory<int>(start - 4);
                }
            }
        }

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
        public virtual void Clear()
        {
            // For an empty struct array, the type doesn't matter
            Memory.WriteMemory(Address, Memory.AllocateStructArray<int>(0).Result);
        }
        public abstract bool Contains(Struct item);
        public abstract void CopyTo(Struct[] array, int arrayIndex);
        /// <summary>
        /// Attemps to free the memory allocated to the array if it's no longer referenced by OMSI.
        /// </summary>
        /// <remarks>
        /// For now this just clears the native array and removes all references so that hopefully the GC can clean it up.
        /// </remarks>
        public virtual void Dispose()
        {
            // TODO: Free the old array
            //Memory.Free(Memory.ReadMemory<int>(Address));
            // Remove references from current array. TODO: Does this work? Is this safe?
            Memory.WriteMemory(Memory.ReadMemory<int>(Address) - 8, 0);
            Memory.WriteMemory(Address, Memory.AllocateStructArray<int>(0, 0).Result);

            GC.SuppressFinalize(this);
        }
        public abstract IEnumerator<Struct> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => WrappedArray.GetEnumerator();
        public abstract int IndexOf(Struct item);
        public abstract void Insert(int index, Struct item);
        public abstract bool Remove(Struct item);
        public abstract void RemoveAt(int index);
        /// <summary>
        /// Forces the cached contents of the MemArray to resynchronise with the hooked application.
        /// </summary>
        /// <param name="index">When set, updates only the item at the given index of the array</param>
        public abstract void UpdateFromHook(int index = -1);

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