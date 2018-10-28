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
    public void Rotate(Quaternion rot)
    {
        vector = rot * vector;
        inTangent = rot * inTangent;
        outTangent = rot * outTangent;
    }
    public void SetVector(Vector3 v)
    {
        var os = v - vector;
        vector = v;
        outTangent += os;
        inTangent += os;
    }
    public void AddVector(Vector3 v)
    {
        vector += v;
        outTangent += v;
        inTangent += v;
    }
    public void SetKey(Key3 k)
    {
        vector = k.vector;
        inTangent = k.inTangent;
        outTangent = k.outTangent;
    }
    public void ScaleInLen(float scale)
    {
        var localIn = inTangent - vector;
        if (localIn.Approx(Vector3.zero))
            inTangent = vector + localIn.normalized * localIn.magnitude * scale;
    }
    public void ScaleOutLen(float scale)
    {
        var localOut = outTangent - vector;
        if (localOut.Approx(Vector3.zero))
            outTangent = vector + localOut.normalized * localOut.magnitude * scale;
    }
    public void Multiply(Matrix4x4 m)
    {
        vector = m.MultiplyPoint(vector);
        inTangent = m.MultiplyPoint(inTangent);
        outTangent = m.MultiplyPoint(outTangent);
    }
    public void Multiply3x4(Matrix4x4 m)
    {
        vector = m.MultiplyPoint3x4(vector);
        inTangent = m.MultiplyPoint3x4(inTangent);
        outTangent = m.MultiplyPoint3x4(outTangent);
    }
    public void Scale(Vector3 scale)
    {
        vector = Vector3.Scale(vector, scale);
        inTangent = Vector3.Scale(inTangent, scale);
        outTangent = Vector3.Scale(outTangent, scale);
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
    public Key3(Vector3 vector, Vector3 inT, Vector3 outT) : this(vector, inT)
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