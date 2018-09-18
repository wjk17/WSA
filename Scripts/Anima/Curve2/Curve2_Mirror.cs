using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Curve2_Tool
{
    /// <summary>
    /// return new Curve contain keys before/include "time", not return new curve Clone
    /// </summary>
    public static Curve2 Before(this Curve2 curve, float time)
    {
        var list = new List<Key2>();
        foreach (var k in curve.keys)
        {
            if (k.time > time) break;
            list.Add(k);
        }
        curve.keys = list;
        return curve;
    }
    /// <summary>
    /// mirror a curve, do return new curve Clone
    /// </summary>
    public static Curve2 Mirror(this Curve2 curve)
    {
        var mir = curve.Clone();
        Mirror(mir, 0.01f);
        return mir;
    }
    public static void Mirror(this Curve2 curve, float mirrorError, Curve2 origin = null)
    {
        var axis = 0.5f;//mirrorAxis
        var append = new List<Key2>();
        foreach (var k in curve)
        {
            if (Mathf.Abs(k.time - axis) < mirrorError)//points on mirror axis
            {
                if (origin != null)
                {
                    var key = origin[curve.IndexOf(k)];
                    key.SetVector(new Vector2(axis, k.value));
                    //key.outMode = key.inMode;
                    //key.outMode = KeyMode.None;
                }
                k.SetVector(new Vector2(axis, k.value));
                k.outTangent = k.inTangent.MirrorX(axis);
                k.outMode = k.inMode;
            }
            else
            {
                var n = k.Clone();
                ComTool.Swap(ref n.inTangent, ref n.outTangent);//先把切线翻转
                n.inTangent = n.inTangent.MirrorX(n.time);
                n.outTangent = n.outTangent.MirrorX(n.time);
                n.SetVector(n.vector.MirrorX(axis)); // 然后SetVector一起拉过去
                ComTool.Swap(ref n.inMode, ref n.outMode);
                append.Add(n);
            }
        }
        curve.keys.AddRange(append);
        curve.Sort();
    }
}
