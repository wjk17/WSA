using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
using Esa.UI_;
public class Items : Singleton<Items>
{
    UIDepot depot;
    public static Item GetItem(int i)
    {
        return I._GetItem(i);
    }
    public Item _GetItem(int i)
    {
        Item item = null;
        ItemSlot srcSlot = null;
        foreach (var slot in depot.slots)
        {
            if (i < slot.capacity)
            {
                item = slot[i];
                srcSlot = slot;
                return item;
            }
            else i -= slot.capacity;
        }
        return null;
    }
    public Item GetItem(string name)
    {
        foreach (var slot in depot.slots)
        {
            foreach (var item in slot.items)
            {
                if (item.name == name) return item;
            }
        }
        return null;
    }
    public override void _Start()
    {
        base._Start();
        depot = GetComponent<UIDepot>();
    }
    void Update()
    {

    }
}
