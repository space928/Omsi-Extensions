using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Wrapper for String Lists in OMSI's Memory.
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="MemArrayBase{Struct}"/>
    /// </remarks>
    public class MemArrayStringList : MemArrayBase<string>
    {
        private StrPtrType stringType = StrPtrType.RawDelphiAnsiString;

        private string[] ReadList()
        {
            uint arr = Memory.ReadMemory<uint>(Address);
            if (arr == 0)
                return Array.Empty<string>();
            uint len = Memory.ReadMemory<uint>(arr + 8);
            uint arrayData = Memory.ReadMemory<uint>(arr + 4);
            string[] ret = new string[len];
            for (uint i = 0; i < len; i++)
            {
                var objAddr = Memory.ReadMemory<int>(arrayData + i * 4);
                if (objAddr == 0)
                {
                    ret[i] = null;
                    continue;
                }

                var n = Memory.ReadMemoryString(objAddr, stringType);
                ret[i] = n;
            }

            return ret;
        }

        private string ReadListItem(int index)
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

            var n = Memory.ReadMemoryString(objAddr, stringType);

            return n;
        }

        /// <summary>
        /// Gets the current contents of the wrapped array. On non-cached arrays this is slow.
        /// </summary>
        public override string[] WrappedArray => cached ? arrayCache : ReadList();

        public MemArrayStringList() : base() { }

        internal MemArrayStringList(Memory memory, int address, bool cached = true, StrPtrType stringType = StrPtrType.RawDelphiAnsiString) : base(memory, address, cached) 
        {
            this.stringType = stringType;
        }

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

        public override string this[int index]
        {
            get => cached ? arrayCache[index] : ReadListItem(index);
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// TODO: Implement efficient enumerator for non-cached arrays.
        /// https://github.com/space928/Omsi-Extensions/issues/110
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<string> GetEnumerator() => ((IEnumerable<string>)WrappedArray).GetEnumerator();

        public override void Add(string item)
        {
            throw new NotSupportedException();
        }

        public override void Clear()
        {
            base.Clear();
            if (cached)
                arrayCache = Array.Empty<string>();
        }

        public override bool Remove(string item) => throw new NotImplementedException();

        public override void Insert(int index, string item) => throw new NotImplementedException();

        public override void RemoveAt(int index) => throw new NotImplementedException();

        public override int IndexOf(string item) => Array.IndexOf(WrappedArray, item);

        public override bool Contains(string item) => WrappedArray.Contains(item);

        public override void CopyTo(string[] array, int arrayIndex) => WrappedArray.CopyTo(array, arrayIndex);
    }
}
