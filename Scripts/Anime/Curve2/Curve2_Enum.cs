using System;
using System.Collections;
using System.Collections.Generic;
public partial class Curve2 : IList<Key2>
{
    public Key2 this[int index]
    {
        get { return ((IList<Key2>)keys)[index]; }
        set { ((IList<Key2>)keys)[index] = value; }
    }
    public int Count { get { return ((IList<Key2>)keys).Count; } }
    public bool IsReadOnly { get { return ((IList<Key2>)keys).IsReadOnly; } }
    public void Add(Key2 item)
    {
        ((IList<Key2>)keys).Add(item);
    }
    public void Clear()
    {
        ((IList<Key2>)keys).Clear();
    }
    public bool Contains(Key2 item)
    {
        return ((IList<Key2>)keys).Contains(item);
    }
    public void CopyTo(Key2[] array, int arrayIndex)
    {
        ((IList<Key2>)keys).CopyTo(array, arrayIndex);
    }
    public IEnumerator<Key2> GetEnumerator()
    {
        return ((IList<Key2>)keys).GetEnumerator();
    }
    public int IndexOf(Key2 item)
    {
        return ((IList<Key2>)keys).IndexOf(item);
    }
    public void Insert(int index, Key2 item)
    {
        ((IList<Key2>)keys).Insert(index, item);
    }
    public bool Remove(Key2 item)
    {
        return ((IList<Key2>)keys).Remove(item);
    }
    public void RemoveAt(int index)
    {
        ((IList<Key2>)keys).RemoveAt(index);
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IList<Key2>)keys).GetEnumerator();
    }
}