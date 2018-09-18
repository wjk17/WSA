using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class CurveXYZ
{
    public CurveXYZ()
    {
        x = new Curve2();
        y = new Curve2();
        z = new Curve2();
    }
    public Curve2 x;
    public Curve2 y;
    public Curve2 z;
}
[Serializable]
public class CurveObj
{
    public CurveObj pair;
    public string name;
    //public CurveObj(ASTransDOF ast) => this.ast = ast;
    public CurveObj(TransDOF ast) { this.ast = ast; pos = new CurveXYZ(); rot = new CurveXYZ(); }
    public TransDOF ast;
    public Curve2[] poss { get { return new Curve2[] { pos.x, pos.y, pos.z }; } }
    public Curve2[] rots { get { return new Curve2[] { rot.x, rot.y, rot.z }; } }
    public CurveXYZ pos;
    public CurveXYZ rot;
    public Curve2[] curves { get { return new Curve2[] { pos.x, pos.y, pos.z, rot.x, rot.y, rot.z }; } }

    public Vector3 Pos(float time)
    {
        return new Vector3(pos.x.Evaluate1D(time), pos.y.Evaluate1D(time), pos.z.Evaluate1D(time));
    }
    public Vector3 Rot(float time)
    {
        return new Vector3(rot.x.Evaluate1D(time), rot.y.Evaluate1D(time), rot.z.Evaluate1D(time));
    }
    public void RemoveAtTime(float time)
    {
        RemoveAtTime(rot.x, time);
        RemoveAtTime(rot.y, time);
        RemoveAtTime(rot.z, time);

        RemoveAtTime(pos.x, time);
        RemoveAtTime(pos.y, time);
        RemoveAtTime(pos.z, time);
    }
    void RemoveTimeOf(Curve2 curve, float time)
    {
        var t = new List<Key2>(curve);
        foreach (var key in t)
        {
            if (MathTool.Approx(key.time, time, 3)) curve.Remove(key);
        }
    }
    void RemoveAtTime(Curve2 curve, float time)
    {
        if (curve.keys != null && curve.keys.Count > 0)
        {
            RemoveTimeOf(curve, time);
        }
    }

    internal Tran2E Tran2E(float time)
    {
        return new Tran2E(Pos(time), Rot(time));
    }

    internal void AddEulerPos(int frameIndex, Vector3 euler, Vector3 pos)
    {
        AddEulerCurve(frameIndex, euler);
        AddPositionCurve(frameIndex, pos);
    }
    public void AddEulerCurve(int frameIndex, Vector3 euler)
    {
        rot.x.Add(new Key2(frameIndex, euler.x));
        rot.y.Add(new Key2(frameIndex, euler.y));
        rot.z.Add(new Key2(frameIndex, euler.z));
    }
    public void AddPositionCurve(int frameIndex, Vector3 pos)
    {
        this.pos.x.Add(new Key2(frameIndex, pos.x));
        this.pos.y.Add(new Key2(frameIndex, pos.y));
        this.pos.z.Add(new Key2(frameIndex, pos.z));
    }
}
