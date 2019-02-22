using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    public static class Cmd_Tool
    {
        //public static bool Contains<T>(this IList<T> list, RectTransform item) where T : CmdHandler
        //{
        //    foreach (T i in list)
        //    {
        //        if (i.owner == item) return true;
        //    }
        //    return false;
        //}
        //public static T Ele<T>(this IList<T> list, RectTransform item) where T : CmdHandler
        //{
        //    foreach (T i in list)
        //    {
        //        if (i.owner == item) return i;
        //    }
        //    return null;
        //}
        // 列表没有则自动添加
        public static T Get<T>(this IList<T> list, object item) where T : CmdHandler, new()
        {
            foreach (T i in list)
            {
                if (i.owner == item) return i;
            }
            //var n = new T();
            //n.owner = item;
            //list.Add(n);
            return null;
        }
    }
}
