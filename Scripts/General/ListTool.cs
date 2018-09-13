using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class ListTool
{
    public static void ConstructList<T>(ref List<T> list, int count) where T : new()
    {
        list = new List<T>();
        for (int i = 0; i < count; i++)
        {
            list.Add(new T());
        }
    }
    public static void ConstructList<T>(IList<T> list) where T : new()
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = new T();
        }
    }
    public static void RemoveDuplicate<T>(params List<T>[] source)
    {
        foreach (var s in source)
        {
            RemoveDuplicate(s);
        }
    }
    public static void RemoveDuplicate<T>(List<T> source)
    {
        var unique = new List<T>();
        var origin = new List<T>(source);
        source.Clear();
        foreach (var i in origin)
        {
            if (unique.Contains(i) == false)
            {
                source.Add(i);
                unique.Add(i);
            }
        }
    }
    public static void BooleanIntersec<T>(out List<T> c, List<T> a, List<T> b)
    {
        c = new List<T>();
        foreach (var i in a)
        {
            if (b.Contains(i)) c.Add(i);
        }
    }
    public static List<T> BooleanIntersec<T>(List<T> a, List<T> b)
    {
        var c = new List<T>();
        foreach (var i in a)
        {
            if (b.Contains(i)) c.Add(i);
        }
        return c;
    }
    public static void BooleanSubject<T>(List<T> main, List<T> b)
    {
        var n = main.ToArray();
        main.Clear();
        foreach (var i in n)
        {
            if (!b.Contains(i)) main.Add(i);
        }
    }
    public static bool Contains<T>(this IList<T> list, T item) where T : class
    {
        foreach (var t in list)
        {
            if (t == item) return true;
        }
        return false;
    }

    public static List<T> Combine<T>(IList<T> list1, IList<T> list2)
    {
        var list = new List<T>();
        foreach (var t in list1)
        {
            list.Add(t);
        }
        foreach (var t in list2)
        {
            list.Add(t);
        }
        return list;
    }
}
