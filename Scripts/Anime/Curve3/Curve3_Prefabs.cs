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
}
