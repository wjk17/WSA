using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public static class Key2_Tool
    {
        public static Vector2 LocalIn(this Key2 k)
        {
            return (k.inTan - k.vector.f);
        }
        public static Vector2 SetLocalIn(this Key2 k, Vector2 v)
        {
            return k.inTan = k.vector + v;
        }
        public static Vector2 LocalOut(this Key2 k)
        {
            return (k.outTan - k.vector.f);
        }
        public static Vector2 SetLocalOut(this Key2 k, Vector2 v)
        {
            return k.outTan = k.vector + v;
        }
        public static void SetLengthOut(this Key2 k, float len)
        {
            var dir = (k.outTan - k.vector.f);
            if (dir != Vector2.zero)
                dir = dir.normalized;
            k.outTan = k.vector + dir * len;
        }
        public static void SetLengthIn(this Key2 k, float len)
        {
            var dir = (k.inTan - k.vector.f);
            if (dir != Vector2.zero)
                dir = dir.normalized;
            k.inTan = k.vector + dir * len;
        }
    }
}