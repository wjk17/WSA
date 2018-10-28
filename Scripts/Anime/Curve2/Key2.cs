using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;

/// TODO 插入时 0.4 0.2 0.4 两端持平
/// 调整time时自动调整保持这个比例
[Serializable]
public class Key2
{
    public float time;
    public float value;
    public Vector2 inTangent;
    public Vector2 outTangent;
    public KeyMode inMode = KeyMode.Bezier;
    public KeyMode outMode = KeyMode.Bezier;
    public Key2()
    {
    }
    public Key2(Vector2 v) : this(v, v, v)
    {
    }
    public Key2(Vector2 v, Vector2 inT, Vector2 outT) : this(v, inT, outT, KeyMode.Bezier, KeyMode.Bezier)
    {
    }
    public Key2(Vector2 v, Vector2 inT, Vector2 outT, KeyMode inMode, KeyMode outMode)
    {
        time = v.x;
        value = v.y;
        inTangent = inT;
        outTangent = outT;
        this.inMode = inMode;
        this.outMode = outMode;
    }
    public Key2(float time, float value)
    {
        this.time = time;
        this.value = value;
        inTangent = vector;
        outTangent = vector;
    }
    public Key2 Clone()
    {
        return new Key2(vector, inTangent, outTangent, inMode, outMode);
    }
    public Vector2 vector
    {
        get { return new Vector2(time, value); }
        set { time = value.x; this.value = value.y; }
    }
    public void SetVector(Vector2 v)
    {
        var os = v - vector;
        vector = v;
        outTangent += os;
        inTangent += os;
    }
}