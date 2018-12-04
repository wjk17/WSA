using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

public static partial class ListTool
{
    public static List<T> RemoveLast<T>(this List<T> list)
    {
        list.RemoveAt(list.Count - 1);
        return list;
    }

    public static IList<T> RemoveLast<T>(this IList<T> list)
    {
        list.RemoveAt(list.Count - 1);
        return list;
    }


    public static T First<T>(this IList<T> list)
    {
        return list[0];
    }
    public static T LastM1<T>(this IList<T> list)
    {
        return list[list.Count - 2];
    }
    public static T Last<T>(this IList<T> list)
    {
        return list[list.Count - 1];
    }
    public static T[] IListToArray<T>(IList<T> list)

    {
        var count = list.Count;
        var ns = new T[count];
        if (list.GetType() == typeof(T[]))
        {
            ns = (T[])list;
        }
        else if (list.GetType() == typeof(List<T>))
        {
            ns = ((List<T>)list).ToArray();
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                ns[i] = list[i];
            }
        }
        return ns;
    }

    public static void Swap<T>(this IList<T> list, int a, int b)
    {
        var t = list[a];
        list[a] = list[b];
        list[b] = t;
    }
}
