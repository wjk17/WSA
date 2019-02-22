using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public static partial class TransTool
    {
        public static void StackChildsL1(this Component com)
        {
            var parent = com.transform;
            var childs = parent.GetChildsL1();
            var prev = parent;
            foreach (var child in childs)
            {
                child.SetParent(prev);
                prev = child;
            }
        }

        public static void ApplyTransform(this Transform t)
        {
            var ts = t.GetTransforms();
            //t.setp
        }
    }
}