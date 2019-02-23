using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
using Esa.UI_;
using System;
public class UIEquip : Singleton<UIEquip>
{
    UIDepot depot;
    public bool debug;
    private void Start()
    {
        depot = this.GetComChild<UIDepot>();
        depot.InitGrid();
        depot.onSwap = OnSwap;
        depot.slots[0].Add(Items.GetItem(0));
        depot.slots[1].Add(Items.GetItem(1));
        depot.slots[2].Add(Items.GetItem(2));        
        depot.Refresh();
    }
    private void OnSwap(Item i, Item o)
    {
        if (debug) print(name +
            " in: " + (i == null ? "null" : i.name) +
            ", out: " + (o == null ? "null" : o.name));
        if (i != null)
        {
            i.objs[0].SetActive(false);
            i.objs[1].SetActive(true);
        }
        if (o != null)
        {
            o.objs[0].SetActive(true);
            o.objs[1].SetActive(false);
        }
    }
}
