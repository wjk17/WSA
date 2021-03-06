﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public static class GOTool
    {
        public static Transform GetOrAddTransScnRoot(string name)
        {
            Transform obj;
            var find = TransTool.SearchScnRootCache(name);
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
}