using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Key3
{
    public Vector3 vector;
    public Vector3 inTangent;
    public Vector3 outTangent;
    public float slope;
    public KeyMode inMode = KeyMode.Bezier;
    public KeyMode outMode = KeyMode.Bezier;
    public void SetVector(Vector3 v)
    {
        var os = v - vector;
        vector = v;
        outTangent += os;
        inTangent += os;
    }
    public Key3() { }
    public Key3(Vector3 vector)
    {
        this.vector = vector;
    }
    public Key3(Vector3 vector, Vector3 inT) : this(vector)
    {
        inTangent = inT;
        inMode = KeyMode.Bezier;
        outMode = KeyMode.None;
    }
    public Key3(Vector3 vector, Vector3 inT, Vector3 outT) : this(vector,inT)
    {
        outTangent = outT;
        outMode = KeyMode.Bezier;
    }
    public Key3 Clone()
    {
        var n = new Key3();
        n.vector = vector;
        n.inTangent = inTangent;
        n.outTangent = outTangent;
        n.slope = slope;
        n.inMode = inMode;
        n.outMode = outMode;
        return n;
    }
}