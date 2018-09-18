using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
[Serializable]
public partial class Curve2
{
    public List<Key2> keys;
    public bool time1D = true; //limit Time Between Vectors In X Axis
    [XmlIgnore] public float approxRange = 0.00001f;
    [XmlIgnore] public static float tangentSlopeCalDeltaX = 0.0000001f;
    public Curve2(Vector2 a, Vector2 b) : this(a, b, true)
    {
    }
    public Curve2(Vector2 a, Vector2 b, bool time1D)
    {
        this.time1D = time1D;
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
}
