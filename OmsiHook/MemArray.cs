using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    public class MemArray<T> : OmsiObject, IEnumerable<T>, ICollection<T>, IList<T> where T : struct
    {
        private T[] arrayCache;

        public MemArray() : base() { }

        internal MemArray(Memory memory, int address) : base(memory, address) { }

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

        public int Count => arrayCache.Length;

        public bool IsReadOnly => true;

        public T this[int index] 
        { 
            get => arrayCache[index];
            set
            {
                arrayCache[index] = value;
                Memory.WriteMemoryArrayItem(Address, value, index);
            }
        }

        public void Add(T item) => throw new NotImplementedException();

        public void Clear() => throw new NotImplementedException();

        public bool Contains(T item) => arrayCache.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => arrayCache.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => (IEnumerator<T>)arrayCache.GetEnumerator();

        public bool Remove(T item) => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => arrayCache.GetEnumerator();

        public int IndexOf(T item) => Array.IndexOf(arrayCache, item);

        public void Insert(int index, T item) => throw new NotImplementedException();

        public void RemoveAt(int index) => throw new NotImplementedException();
    }
}
