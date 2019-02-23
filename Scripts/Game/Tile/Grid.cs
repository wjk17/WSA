using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa.UI_;
using Esa;
using System;
using System.Xml.Serialization;
[Serializable]
public class GridUnit
{
    public GridUnit() { }
    public GridUnit(int id, Vector3Int pos)
    {
        this.id = id;
        this.pos = pos;
    }
    public GridUnit(int id, Vector3Int pos, GameObject go) : this(id, pos)
    {
        this.go = go;
    }
    public Vector3Int pos; // 在网格中的位置
    public Vector3 size = Vector3.one; // 规格化尺寸，实际尺寸 = size * 格子尺寸
    public int id; // 单位，物品代号
    [XmlIgnore]
    public GameObject go; // 对象
}
[Serializable]
public class GridSized // 3维稀疏网格
{
    public GridSized() { }
    public List<GridUnit> units;
    public Vector3 gridUnitSize;
    public bool coverLocalScale;
    public static bool debug = false;

    // 待做：排序后使用查找算法优化索引器
    public int SortList(GridUnit ua, GridUnit ub)
    {
        var a = ua.pos;
        var b = ub.pos;
        // 顺序从低到高，从x到z
        if (a.x > b.x) return 1;
        else if (a.x < b.x) return -1;
        else
        {
            if (a.y > b.y) return 1;
            else if (a.y < b.y) return -1;
            else
            {
                if (a.z > b.z) return 1;
                else if (a.z < b.z) return -1;
                else return 0;
            }
        }
    }
    public void Arrange()
    {
        foreach (var u in units)
        {
            if (u == null || u.id < 0) continue;
            u.go.transform.position = Vector3.Scale(u.pos, gridUnitSize);
            if (coverLocalScale) u.go.transform.localScale = gridUnitSize;
        }
    }
    public void Add(GridUnit u)
    {
        u.go.transform.position = Vector3.Scale(u.pos, gridUnitSize);
        if (coverLocalScale) u.go.transform.localScale = gridUnitSize;
        units.Add(u);
        units.Sort(SortList);
    }
    public void Clear()
    {
        foreach (var u in units)
        {
            ComTool.DestroyAuto(u.go);
        }
        units.Clear();
    }
    // 使用位置可以优化搜索
    public void Remove(Vector3Int pos)
    {
        units.RemoveAt(IndexOf(pos));
    }
    public void Remove(GridUnit unit)
    {
        units.Remove(unit);
    }
    public int IndexOf(Vector3Int pos)
    {
        int i = 0;
        foreach (var unit in units)
        {
            if (unit.pos == pos)
            {
                if (unit.id > -1) // -1 即存在过单位但被销毁了
                    return i;
                else
                {
                    if (debug) Debug.Log("pos: " + pos + " not found.");
                    return -1;
                }
            }
            i++;
        }
        return -1;
    }
    public GridUnit this[GameObject go]
    {
        get
        {
            if (go == null) Debug.Log("go == null.");
            foreach (var unit in units)
            {
                if (unit.go == go) return unit;
            }
            if (debug) Debug.Log("go: " + go.name + " not found.");
            return null;
        }
    }
    public GridUnit this[Vector3Int pos]
    {
        get
        {
            foreach (var unit in units)
            {
                if (unit.pos == pos)
                {
                    if (unit.id > -1) // -1 即存在过单位但被销毁了
                        return unit;
                    else
                    {
                        if (debug) Debug.Log("pos: " + pos + " not found.");
                        return null;
                    }
                }
            }
            if (debug) Debug.Log("pos: " + pos + " not found.");
            return null;
        }
    }
}