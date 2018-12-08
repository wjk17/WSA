using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa
{
    [Serializable]
    public struct Vector2if
    {
        public Vector2Int i;
        public Vector2 f;
        public int xi { set { f.x = i.x = value; } get { return i.x; } }
        public int yi { set { f.y = i.y = value; } get { return i.y; } }
        public float xf { set { i.x = Mathf.RoundToInt(f.x = value); } get { return f.x; } }
        public float yf { set { i.y = Mathf.RoundToInt(f.y = value); } get { return f.y; } }

        public static implicit operator Vector2if(Vector2 v)
        {
            var vif = new Vector2if();
            vif.i = v.RoundToInt();
            vif.f = v;
            return vif;
        }
        public static implicit operator Vector2(Vector2if vif)
        {
            return vif.f;
        }
        public static implicit operator Vector2Int(Vector2if vif)
        {
            return vif.i;
        }
    }
}