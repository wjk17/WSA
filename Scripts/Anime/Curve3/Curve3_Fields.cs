using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System;
[Serializable]
public partial class Curve3
{
    public List<Key3> keys;
    [XmlIgnore] public float approxRange = 0.00001f;
    [XmlIgnore] public static float tangentSlopeCalDeltaX = 0.0000001f;
    public Curve3(Vector3[] ps) : this(ps[0], ps[3])
    {
        keys[0].outTangent = ps[1];
        keys[1].inTangent = ps[2];
    }
    public Curve3(Vector3 a, Vector3 b, Vector3 a_outT, Vector3 b_inT) : this(a, b)
    {
        keys[0].outTangent = a_outT;
        keys[1].inTangent = b_inT;
    }
    public Curve3(Vector3 a, Vector3 b) : this()
    {
        keys.Add(new Key3(a));
        keys.Add(new Key3(b));
        keys[0].inMode = KeyMode.None;
        keys[1].outMode = KeyMode.None;
    }
    public Curve3()
    {
        keys = new List<Key3>();
        listStartTime = new List<float>();
        listEndTime = new List<float>();
    }
    public bool hasKey
    {
        get { return keys != null && keys.Count > 0; }
    }
    public bool IsMirror(Curve3 c)
    {
        if (c == null) return false;
        if (Count != c.Count) return false;
        for (int i = 0; i < Count; i++)
        {
            if (!keys[i].vector.IsMirrorX(c.keys[i].vector) ||
                !keys[i].inTangent.IsMirrorX(c.keys[i].inTangent) ||
                !keys[i].outTangent.IsMirrorX(c.keys[i].outTangent) ||
                keys[i].inMode != c.keys[i].inMode ||
                keys[i].outMode != c.keys[i].outMode)
                return false;
        }
        return true;
    }
    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        var c = (Curve3)obj;
        if (Count != c.Count) return false;
        for (int i = 0; i < Count; i++)
        {
            if (!keys[i].vector.Approx(c.keys[i].vector) ||
                !keys[i].inTangent.Approx(c.keys[i].inTangent) ||
                !keys[i].outTangent.Approx(c.keys[i].outTangent) ||
                keys[i].inMode != c.keys[i].inMode ||
                keys[i].outMode != c.keys[i].outMode)
                return false;
        }
        return true;
    }
}