﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Esa
{
    public static partial class ListTool
    {
        public static T[] CloneArray<T>(this T[] array) where T : new()
        {
            var ar = new T[array.Length];
            Array.Copy(array, ar, array.Length);
            return ar;
        }

        // 根据指定序号重新排序
        public static T[] Arrange<T>(this T[] list, params int[] idx)
        {
            var vs = new T[list.Length];
            for (int i = 0; i < idx.Length; i++)
            {
                vs[i] = list[idx[i]];
            }
            return vs;
        }
        public static List<T> Arrange<T>(this List<T> list, params int[] idx)
        {
            var vs = new List<T>();
            for (int i = 0; i < idx.Length; i++)
            {
                vs.Add(list[idx[i]]);
            }
            return vs;
        }
        // 设置Z值
        public static Vector3[] SetZ(this Vector2[] vs, float z)
        {
            var v3 = new Vector3[vs.Length];
            for (int i = 0; i < vs.Length; i++)
            {
                v3[i] = vs[i].SetZ(z);
            }
            return v3;
        }
        public static List<Vector3> SetZ(this List<Vector2> vs, float z)
        {
            var v3 = new List<Vector3>();
            foreach (var v in vs)
            {
                v3.Add(v.SetZ(z));
            }
            return v3;
        }
        public static int IndexOfApprox(this IList<Vector3> vs, Vector3 target, float dist)
        {
            int i = 0;
            foreach (var v in vs)
            {
                if (Vector3.Distance(v, target) < dist) return i;
                i++;
            }
            return -1;
        }
        public static bool ContainApprox(this IList<Vector3> vs, Vector3 target, float dist)
        {
            foreach (var v in vs)
            {
                if (Vector3.Distance(v, target) < dist) return true;
            }
            return false;
        }
        public static string ToStr<T>(this IList<T> list, string split = "")
        {
            string str = "";
            foreach (var item in list)
            {
                if (str.Length != 0) str += split;
                str += item.ToString();
            }
            return str;
        }
        public static List<string> ToStrList<T>(this IList<T> list)
        {
            var strs = new List<string>();
            foreach (var item in list)
            {
                strs.Add(item.ToString());
            }
            return strs;
        }
        public static bool Contains<T>(this T[] array, T t)
        {
            foreach (var item in array)
            {
                if (t.Equals(item)) return true;
            }
            return false;
        }
        public static RectTransform[] ToRTs(this MonoBehaviour[] monos)
        {
            var rts = new List<RectTransform>();
            foreach (var mono in monos)
            {
                rts.Add(mono.transform as RectTransform);
            }
            return rts.ToArray();
        }
        public static RectTransform[] ToRTs(this Transform[] ts)
        {
            var rts = new List<RectTransform>();
            foreach (var t in ts)
            {
                rts.Add(t as RectTransform);
            }
            return rts.ToArray();
        }
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
        public static List<T> MemsClone<T>(this List<T> list) where T : ICloneable
        {
            var n = new List<T>();
            foreach (var item in list)
            {
                n.Add((T)item.Clone());
            }
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
        public static List<T> Reverse_<T>(this List<T> list)
        {
            list.Reverse();
            return list;
        }
        public static T Add_<T>(this List<T> list, T item)
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
        public static List<T> N_Reverse<T>(this List<T> list1)
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
}