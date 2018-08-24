using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GOTool
{
    public static Transform GetOrAddTransScnRoot(string name)
    {
        Transform obj;
        var find = TransformTool.SearchScnRootCache(name);
        if (find != null) obj = find;
        else
        {
            var c = new GameObject(name);
            obj = c.transform;
        }
        return obj;        
    }
    public static Transform GetOrAddTrans(this Transform parent, string name)
    {
        Transform obj;
        var find = parent.SearchCache(name);
        if (find != null) obj = find;
        else
        {
            var c = new GameObject(name);
            obj = c.transform;
            obj.SetParent(parent);
        }
        return obj;
    }
}
