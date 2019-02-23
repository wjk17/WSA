using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Esa;
using Esa.UI_;
using System;
public class UIDepot : Singleton<UIDepot>
{
    public List<ItemSlot> slots;
    public UIGrid grid;
    UIGrid _grid;
    public UIDepot connectDepot;
    public Action<Item, Item> onSwap; // in, out
    public void InitGrid(bool refresh = false)
    {
        grid = this.GetComChild<UIGrid>();
        grid.Initialize();
        grid.onClick = OnClick;
        grid.onOver = OnOver;
        if (refresh) Refresh();
        this.AddInput(_Input, -1, false);
        this.DestroyImages();
    }
    private void _Input()
    {
        this.BeginOrtho();
        this.DrawBG();
    }

    ItemSlot GetSlot(ItemType type)
    {
        foreach (var slot in slots)
        {
            if ((slot.type & type) != 0)
                return slot;
        }
        return null;
    }

    /// <summary>
    /// return a item to switch if already exist, or add to tail then return null.
    /// </summary>
    /// 在slot里找一个能放进source slot的物品
    public Item Swap(Item item, ItemSlot srcSlot)
    {
        // 先找到能放下该类Item的 slot
        var slot = GetSlot(item.type);
        if (slot == null)
        {
            if (SYS.debugDepot) print("Failed Dest Slot Not Found.");
            return item; // 没有对应插槽，返回原item，操作失败
        }
        if (slot.Full) // switch
        {
            // 找一个能放在 源Slot 的item来交换
            var i = slot.IndexOf(srcSlot.type);
            if (i < 0)
            {
                if (SYS.debugDepot) print("Failed Src Slot Not Found.");
                return item; // 找不到则返回原item，操作失败
            }
            if (SYS.debugDepot) print("Successed Swap");
            var swap = slot[i]; // 找到返回交换item，成功
            slot[i] = item;
            Refresh();
            OnSwap(item, swap);
            return swap;
        }
        else // add
        {
            if (SYS.debugDepot) print("Successed Add");
            slot.Add(item);
            srcSlot.Remove(item);
            Refresh();
            OnSwap(item, null);
            return null;
        }
    }
    private void OnOver(int i)
    {
        Item item = null;
        ItemSlot srcSlot = null;
        int n = i;
        foreach (var slot in slots)
        {
            if (i < slot.capacity)
            {
                item = slot[i];
                srcSlot = slot;
                break;
            }
            else i -= slot.capacity;
        }
        if (item != null)
        {
            UI_Desc.I.gameObject.SetActive(true);
            var rt = UI_Desc.I.transform as RectTransform;
            rt.anchoredPosition = UI.mousePosRef;
            if (UI.mousePosRef.x < rt.rect.size.x)
                rt.AddUIPosX(rt.rect.size.x);
            UI_Desc.I.name = item.name;
            UI_Desc.I.desc = item.desc;
        }
    }
    void OnClick(int i)
    {
        Item item = null;
        ItemSlot srcSlot = null;
        int n = i;
        foreach (var slot in slots)
        {
            //if (SYS.debugDepot) print("Reach: " + i + " Slot: " + slot.name + " " + slot.capacity);
            if (i < slot.capacity)
            {
                item = slot[i];
                srcSlot = slot;
                break;
            }
            else i -= slot.capacity;
        }
        if (SYS.debugDepot) print("OnClick: " + n + " Slot: " + srcSlot.name + ", Item: " + item.name);
        if (item == null) throw new Exception("Out of bound");
        var swap = connectDepot.Swap(item, srcSlot);
        if (swap != item) // success                 
        {
            OnSwap(swap, item);
            Refresh();
        }
    }
    void OnSwap(Item i, Item o)
    {
        if (onSwap != null) onSwap(i, o);
    }
    [Button]
    public void Refresh()
    {
        grid.clickable = new List<bool>();
        grid.names = new List<string>();
        grid.textures = new List<Texture2D>();
        for (int i = 0; i < grid.gridCount.eleCount(); i++)
        {
            grid.clickable.Add(false);
            grid.names.Add("");
            grid.textures.Add(null);
        }
        int idx = 0;
        foreach (var slot in slots)
        {
            for (int i = 0; i < slot.Count; i++)
            {
                grid.clickable[idx + i] = true;
                grid.names[idx + i] = slot[i].name;
                grid.textures[idx + i] = slot[i].thumbnail;
            }
            idx += slot.capacity;
        }
    }
}