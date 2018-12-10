using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
namespace Esa
{
    [Serializable]
    public partial class Curve2
    {
        public List<Key2> keys;
        public bool time1D; //limit Time Between Vectors In X Axis
        [XmlIgnore] public float approxRange = 0.00001f;
        [XmlIgnore] public static float tangentSlopeCalDeltaX = 0.0000001f;
        public Curve2(Vector2 a, Vector2 b)
        {
            keys = new List<Key2>();
            InsertKey(new Key2(a));
            InsertKey(new Key2(b));
        }
        public Curve2() { keys = new List<Key2>(); }
        public Curve2(List<Key2> keys) { this.keys = keys; }
        public bool hasKey
        {
            get { return keys != null && keys.Count > 0; }
        }
        public Key2 IdxOf(int idx)
        {
            if (!hasKey) return null;
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].idx == idx) { return keys[i]; }
            }
            return null;
        }
        public int TimeOf(float time)
        {
            if (!hasKey) return -1;
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].time == time) { return i; }
            }
            return -1;
        }
        public Curve2 Clone()
        {
            var n = new Curve2();
            var nKeys = new List<Key2>();
            foreach (var k in keys)
            {
                nKeys.Add(k.Clone());
            }
            n.keys = nKeys;
            n.time1D = time1D;
            return n;
        }
        public bool IsMirror(Curve2 c)
        {
            if (c == null) return false;
            if (Count != c.Count) return false;
            for (int i = 0; i < Count; i++)
            {
                if (!keys[i].vector.f.IsMirrorX(c.keys[i].vector) ||
                    !keys[i].inTan.IsMirrorX(c.keys[i].inTan) ||
                    !keys[i].outTan.IsMirrorX(c.keys[i].outTan) ||
                    keys[i].inMode != c.keys[i].inMode ||
                    keys[i].outMode != c.keys[i].outMode)
                    return false;
            }
            return true;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var c = (Curve2)obj;
            if (Count != c.Count) return false;
            for (int i = 0; i < Count; i++)
            {
                if (!keys[i].vector.f.Approx(c.keys[i].vector) ||
                    !keys[i].inTan.Approx(c.keys[i].inTan) ||
                    !keys[i].outTan.Approx(c.keys[i].outTan) ||
                    keys[i].inMode != c.keys[i].inMode ||
                    keys[i].outMode != c.keys[i].outMode)
                    return false;
            }
            return true;
        }
    }
}