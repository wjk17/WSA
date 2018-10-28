using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Curve3
{
    static Curve3()
    {
        t0v0_t1v0 = new Curve3(Vector3.zero, Vector3.forward,
            Vector3.forward * 0.3333f,
            Vector3.forward * 0.6667f);
    }
    static Curve3 t0v0_t1v0;
    public static Curve3 T0V0_T1V0 { get { return t0v0_t1v0.Clone(); } }

    public Curve3 Clone()
    {
        var n = new Curve3();
        var nKeys = new List<Key3>();
        foreach (var k in keys)
        {
            nKeys.Add(k.Clone());
        }
        n.keys = nKeys;
        return n;
    }

    internal void Subdivide()
    {
        if (Count != 2) return;
        var a = keys[0];
        var b = keys[1];
        var v = Evaluate(a, b, 0.5f);
        var i = Evaluate(a, b, 0.25f);
        var o = Evaluate(a, b, 0.75f);
        var k = new Key3(v, i, o);
        a.ScaleOutLen(0.5f);
        b.ScaleInLen(0.5f);
        Insert(1, k);
    }

    internal void ClearSlope()
    {
        foreach (var key in keys)
        {
            key.slope = 0;
        }
    }
}
