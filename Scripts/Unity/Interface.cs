using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public interface IFindScnRoot
    {
        string findName { get; }
        Transform transform { get; }
    }

    /// <summary>
    /// IFind使用了缓存搜索，在缓存记录中的对象即使脱离原对象层级，也会被当做缓存使用
    /// </summary>
    public interface IFind
    {
        string findName { get; }
        Transform transform { get; }
    }
    public static class IFindExtend
    {
        //IFind
        public static void Clear(this IFind fc)
        {
            fc.transform.ClearChildren();
        }
        public static Transform Find(this IFind fc)
        {
            return GOTool.GetOrAddTrans(fc.transform, fc.findName);
        }
        //IFindRootExtend
        public static Transform Find(this IFindScnRoot fc)
        {
            return GOTool.GetOrAddTransScnRoot(fc.findName);
        }
    }
}