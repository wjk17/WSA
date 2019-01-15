using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
namespace Esa
{
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
        public static void IListCopyTo<T>(this IList<T> source, IList<T> dst)
        {
            for (int i = 0; i < source.Count; i++)
            {
                dst[i] = source[i];
            }
        }
        public static T[] IListToArray<T>(IList<T> list)
        {
            if (list == null) return null;
            var count = list.Count;
            // 是 Array 直接返回
            if (list.GetType() == typeof(T[]))
            {
                return (T[])list;
            }
            // List 转 Array
            else if (list.GetType() == typeof(List<T>))
            {
                return ((List<T>)list).ToArray();
            }
            // 其他 IList 转 Array
            else
            {
                var ns = new T[count];
                for (int i = 0; i < count; i++)
                {
                    ns[i] = list[i];
                }
                return ns;
            }
        }

        public static void Swap<T>(this IList<T> list, int a, int b)
        {
            var t = list[a];
            list[a] = list[b];
            list[b] = t;
        }
    }
}