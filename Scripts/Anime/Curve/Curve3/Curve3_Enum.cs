using System;
using System.Collections;
using System.Collections.Generic;
namespace Esa
{
    public partial class Curve3 : IList<Key3>
    {
        public Key3 this[int index]
        {
            get { return ((IList<Key3>)keys)[index]; }
            set { ((IList<Key3>)keys)[index] = value; }
        }
        public int Count { get { return ((IList<Key3>)keys).Count; } }
        public bool IsReadOnly { get { return ((IList<Key3>)keys).IsReadOnly; } }
        public void Add(Key3 item)
        {
            ((IList<Key3>)keys).Add(item);
        }
        public void Clear()
        {
            ((IList<Key3>)keys).Clear();
        }
        public bool Contains(Key3 item)
        {
            return ((IList<Key3>)keys).Contains(item);
        }
        public void CopyTo(Key3[] array, int arrayIndex)
        {
            ((IList<Key3>)keys).CopyTo(array, arrayIndex);
        }
        public IEnumerator<Key3> GetEnumerator()
        {
            return ((IList<Key3>)keys).GetEnumerator();
        }
        public int IndexOf(Key3 item)
        {
            return ((IList<Key3>)keys).IndexOf(item);
        }
        public void Insert(int index, Key3 item)
        {
            ((IList<Key3>)keys).Insert(index, item);
        }
        public bool Remove(Key3 item)
        {
            return ((IList<Key3>)keys).Remove(item);
        }
        public void RemoveAt(int index)
        {
            ((IList<Key3>)keys).RemoveAt(index);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<Key3>)keys).GetEnumerator();
        }
    }
}