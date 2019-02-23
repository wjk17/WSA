using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
using System;

public class UIBag : Singleton<UIBag>
{
    UIDepot depot;
    public bool debug;
    public int[] idxs;
    private void Start()
    {
        depot = this.GetComChild<UIDepot>();
        depot.InitGrid();
        var slot = new ItemSlot(ItemType.WholeBody, depot.grid.gridCount.eleCount());
        foreach (var idx in idxs)
        {
            slot.Add(Items.GetItem(idx));
        }
        depot.slots = new List<ItemSlot>();
        depot.slots.Add(slot);
        depot.onSwap = OnSwap;
        depot.Refresh();
    }
    private void OnSwap(Item i, Item o)
    {
        if (debug) print(name +
            " in: " + (i == null ? "null" : i.name) +
            ", out: " + (o == null ? "null" : o.name));
    }
}
