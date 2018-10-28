using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StrCom = System.StringComparison;
/// <summary>
/// 使用缓存池的搜索，提升二次搜索的效率
/// </summary>
public static partial class TransformTool
{
    static TransformTool()
    {
        cachePools = new List<Pool>();
        rootPool = new Pool();
    }
    /// <summary>
    /// exclude itself
    /// </summary>
    public static List<Vector3> GetPoss<T>(this T transform) where T : Component
    {
        var vs = new List<Vector3>();
        foreach (var t in transform.GetComponentsInChildren<Transform>(true))
        {
            vs.Add(t.position);
        }
        vs.RemoveAt(0);
        return vs;
    }
    public static List<Vector3> GetLocalPoss<T>(this T transform) where T : Component
    {
        var vs = new List<Vector3>();
        foreach (var t in transform.GetComponentsInChildren<Transform>(true))
        {
            vs.Add(t.localPosition);
        }
        vs.RemoveAt(0);
        return vs;
    }
    public static void FlatChildrens(this Component t)
    {
        foreach (var c in t.GetTransforms())
        {
            c.SetParent(t.transform, true);
        }
    }
    public static List<T> GetComs<T>(this Transform trans, List<string> names) where T : Component
    {
        var ts = trans.GetChildsL1();
        var list = new List<T>();
        foreach (var t in ts)
        {
            if (names.Contains(t.name)) { list.Add(t.GetComponent<T>()); names.Remove(t.name); }
        }
        return list;
    }
    public static List<Transform> GetTransforms(this Transform T, List<string> names)
    {
        var ts = T.GetChildsL1();
        var list = new List<Transform>();
        foreach (var t in ts)
        {
            if (names.Contains(t.name)) { list.Add(t); names.Remove(t.name); }
        }
        return list;
    }
    public static List<Transform> GetTransforms<T>(this T t) where T : Component
    {
        var ts = new List<Transform>();
        t.GetComponentsInChildren(true, ts);
        ts.RemoveAt(0);
        return ts;
    }
    public static List<T> GetChildrens<T>(this Component t) where T : Component
    {
        var ts = new List<T>();
        t.GetComponentsInChildren(true, ts);
        var c = t.GetComponent<T>();
        if (ts != null && c != null) ts.Remove(c);
        return ts;
    }
    public static List<T> GetChildrens<T>(this T t) where T : Component
    {
        var ts = new List<T>();
        t.GetComponentsInChildren(true, ts);
        ts.RemoveAt(0);
        return ts;
    }
    //public static List<T> GetChildrensL1<T>(this T t) where T : Component
    //{
    //    return null;
    //}
    public static List<Transform> GetChildsL1(this Transform t)
    {
        return t.GetChilds(1);
        //var list = new List<Transform>();
        //for (int i = 0; i < t.childCount; i++)
        //{
        //    list.Add(t.GetChild(i));
        //}
        //return list;
    }
    public static List<Transform> GetChilds(this Transform t, int level, int curr = 0)//层级
    {
        var list = new List<Transform>();
        if (++curr > level) return list;
        for (int i = 0; i < t.childCount; i++)
        {
            if (curr == level) list.Add(t.GetChild(i));
            else list.AddRange(GetChilds(t.GetChild(i), level, curr));
        }
        return list;
    }
    public static Transform Find(this List<Transform> ts, string name)
    {
        var mode = m_IgnoreCase ? StrCom.OrdinalIgnoreCase : StrCom.Ordinal;
        foreach (var t in ts)
        {
            if (t != null && t.name.Equals(name, mode)) return t;
        }
        return null;
    }
    public static Transform FindRootCache(string name)
    {
        var mode = m_IgnoreCase ? StrCom.OrdinalIgnoreCase : StrCom.Ordinal;
        foreach (var t in rootPool.children)
        {
            if (t != null && t.name.Equals(name, mode)) return t;
        }
        return null;
    }
    public static Transform FindCache(Transform parent, string name)
    {
        var mode = m_IgnoreCase ? StrCom.OrdinalIgnoreCase : StrCom.Ordinal;
        var p = GetPool(parent);
        if (p == null) return null;
        foreach (var t in p.children)
        {
            if (t != null && t.name.Equals(name, mode)) return t;
        }
        return null;
    }
    public class Pool
    {
        public Pool() { children = new List<Transform>(); }
        public Transform parent;
        public List<Transform> children;
    }
    static void AddCache(Transform owner, Transform target)
    {
        var pool = GetOrAddPool(owner);
        pool.children.Add(target);
    }
    static Pool GetOrAddPool(Transform parent)
    {
        foreach (var pool in cachePools)
        {
            if (pool.parent == parent) return pool;
        }
        var p = new Pool();
        cachePools.Add(p);
        return p;
    }
    static Pool GetPool(Transform parent)
    {
        foreach (var pool in cachePools)
        {
            if (pool.parent == parent) return pool;
        }
        return null;
    }
    static List<Pool> cachePools;
    static Pool rootPool;
    public static Transform SearchScnRootCache(string name)
    {
        var r = FindRootCache(name);
        if (r != null) return r;
        else r = SearchScnRoot(name);
        if (r != null) rootPool.children.Add(r);
        return r;
    }
    public static Transform SearchScnCache(this Transform parent, string name)
    {
        var r = FindCache(parent, name);
        if (r != null) return r;
        else r = SearchScn(name);
        if (r != null) AddCache(parent, r);
        return r;
    }
    public static Transform SearchCache(this Transform parent, string name)
    {
        var r = FindCache(parent, name); // 先从池里找
        if (r != null) return r;
        else r = Search(parent, name); // 池里没有则使用Search
        if (r != null) AddCache(parent, r); // 更新缓存池，索引是搜索的目标的Transform组件指针
        return r;
    }
}
