using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Wrapper for arrays of strings in OMSI's Memory with automatic index caching.
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="MemArrayBase{Struct}"/>
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
