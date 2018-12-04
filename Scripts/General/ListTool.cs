using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
public static partial class ListTool
{
    public static int ClampIdx<T>(this List<T> list, int idx)
    {
        return Mathf.Clamp(idx, 0, list.Count - 1);
    }
    public static T GetByName<T>(this List<T> list, string name) where T : Object
    {
        foreach (var item in list)
        {
            if (item.name == name) return item;
        }
        return null;
    }
    public static List<T> Clone<T>(this List<T> list)
    {
        var n = new List<T>(list);
        return n;
    }
    public static List<Vector2> Remap(this List<Vector2> list)
    {
        return Remap(list, Vector2.zero, Vector2.one);
    }
    public static List<Vector2> Remap(this List<Vector2> list, Vector2 max)
    {
        return Remap(list, Vector2.zero, max);
    }
    public static List<Vector2> Remap(this List<Vector2> list, Vector2 min, Vector2 max)
    {
        var _max = Vector2.negativeInfinity;
        var _min = Vector2.positiveInfinity;
        foreach (var i in list)
        {
            _max = Vector2.Max(i, _max);
            _min = Vector2.Min(i, _min);
        }

        for (int i = 0; i < list.Count; i++)
        {
            list[i] = min.Lerp2(max, (list[i] - _min).Divide(_max - _min));
        }
        return list;
    }
    public static T[] Copy<T>(this T[] list)
    {
        return list.Empty() ? null : new List<T>(list).ToArray();
    }
    public static List<T> CloneMw<T>(this List<T> list) where T : ICloneable
    {
        var n = new List<T>();
        foreach (var i in list)
        {
            n.Add((T)i.Clone());
        }
        return n;
    }
    public static List<T> Copy<T>(this List<T> list)
    {
        return list.Empty() ? null : new List<T>(list);
    }
    public static List<string> GetNames<T>(this IList<T> coms) where T : Component
    {
        var ns = new List<string>();
        foreach (var com in coms)
        {
            ns.Add(com.name);
        }
        return ns;
    }
    public static bool NotEmpty<T>(this IList<T> fs)
    {
        return fs != null && fs.Count > 0;
    }
    public static bool Empty<T>(this IList<T> fs)
    {
        return fs == null || fs.Count == 0;
    }
    public static List<float> _sub(this List<float> fs, float f)
    {
        var n = new List<float>(fs);
        for (int i = 0; i < fs.Count; i++)
        {
            n[i] = f - fs[i];
        }
        return n;
    }
    public static List<float> _add(this List<float> fs, float f)
    {
        var n = new List<float>(fs);
        for (int i = 0; i < fs.Count; i++)
        {
            n[i] += f;
        }
        return n;
    }

    public static T Add_R<T>(this List<T> list, T item)
    {
        list.Add(item);
        return item;
    }
    public static List<T> AndSet<T>(this IList<T> list1, IList<T> list2, Func<IList<T>, T, bool> contains)
    {
        var list3 = new List<T>();
        list3.AddRange(list1);
        foreach (var i in list2)
        {
            if (!contains(list1, i))
                list3.Add(i);
        }
        return list3;
    }
    public static int IdxOf<T>(this IList<T> objs, string name) where T : UnityEngine.Object
    {
        for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i].name == name) return i;
        }
        return -1;
    }

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
    public static List<T> Rever<T>(this List<T> list1)
    {
        var n = new List<T>(list1);
        n.Reverse();
        return n;
    }
    public static List<T> Combine<T>(this IList<T> list1, IList<T> list2)
    {
        var list = new List<T>();
        list.AddRange(list1);
        list.AddRange(list2);
        return list;
    }
}
