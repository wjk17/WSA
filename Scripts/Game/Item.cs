using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Flags]
public enum ItemType
{
    Disable,
    Upper = 1,
    Lower = 2,
    Shoes = 4,
    UpperBody = Upper,
    LowerBody = Lower | Shoes,
    WholeBody = Upper | Lower | Shoes,
}
[Serializable]
public class Item
{
    public string name;
    public string desc;
    public GameObject[] objs;
    public ItemType type;
    public Texture2D thumbnail;
}
[Serializable]
public class ItemSlot : IList<Item>
{
    public List<Item> items;
    public ItemType type;
    public int capacity;
    public string name;

    public ItemSlot(ItemType type, int capacity)
    {
        items = new List<Item>();
        this.type = type;
        this.capacity = capacity;
    }
    public ItemSlot()
    {
        items = new List<Item>();
    }
    //public Item this[int index] { get => ((IList<Item>)items)[index]; set => ((IList<Item>)items)[index] = value; }
    public Item this[int index]
    {
        get { return ((IList<Item>)items)[index]; }
        set { ((IList<Item>)items)[index] = value; }
    }

    public bool Full
    {
        get { return items.Count == capacity; }
    }

    //public int Count => ((IList<Item>)items).Count;
    public int Count { get { return ((IList<Item>)items).Count; } }

    //public bool IsReadOnly => ((IList<Item>)items).IsReadOnly;
    public bool IsReadOnly { get { return ((IList<Item>)items).IsReadOnly; } }

    public void Add(Item item)
    {
        ((IList<Item>)items).Add(item);
    }

    public void Clear()
    {
        ((IList<Item>)items).Clear();
    }

    public bool Contains(Item item)
    {
        return ((IList<Item>)items).Contains(item);
    }

    public void CopyTo(Item[] array, int arrayIndex)
    {
        ((IList<Item>)items).CopyTo(array, arrayIndex);
    }

    public IEnumerator<Item> GetEnumerator()
    {
        return ((IList<Item>)items).GetEnumerator();
    }

    public Item GetItem(ItemType type)
    {
        foreach (var item in items)
        {
            if (item.type == type) return item;
        }
        return null;
    }
    public int IndexOf(ItemType type)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if ((items[i].type & type) != 0) return i;
        }
        return -1;
    }

    public int IndexOf(Item item)
    {
        return ((IList<Item>)items).IndexOf(item);
    }

    public void Insert(int index, Item item)
    {
        ((IList<Item>)items).Insert(index, item);
    }

    public bool Remove(Item item)
    {
        return ((IList<Item>)items).Remove(item);
    }

    public void RemoveAt(int index)
    {
        ((IList<Item>)items).RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IList<Item>)items).GetEnumerator();
    }
}
