using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct Rt
{
    public Vector2 pos;
    public Vector2 size;
    public Vector2 anchor;
    public Rt(Vector2 p, float sideLength) : this(p, Vector2.one * sideLength)
    {
    }
    public Rt(Vector2 p, float sideLength, Vector2 anchor) : this(p, Vector2.one * sideLength, anchor)
    {
    }
    public Rt(Vector2 p, Vector2 size)
    {
        this.pos = p;
        this.size = size;
        anchor = Vector2.zero;
    }
    public Rt(Vector2 p, Vector2 size, Vector2 anchor) : this(p, size)
    {
        this.anchor = anchor;
    }
    public bool Contains(Vector2 v)
    {
        var lt = pos - size * anchor;
        var rb = pos + size * (Vector2.one - anchor);
        return MathTool.Between(v, lt, rb);
    }
    //public List<Vector2> ToPoints()
    //{
    //    var lt = p - size * 0.5f;
    //    var lb = p - size * 0.5f;
    //    var rb = p + size * 0.5f;
    //}


    static bool Approx(float a, float b)
    {
        return Mathf.Approximately(a, b);
    }
    static bool Approx(Vector2 a, Vector2 b)
    {
        return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);
    }
    static Vector2 ReverseY(Vector2 a)
    {
        return a * new Vector2(1, -1);
    }
    public Rt(RectTransform t)
    {
        var amin = t.anchorMin;
        var amax = t.anchorMax;
        var parent = t.parent as RectTransform;
        // min 0,0  max 1,1
        if (Approx(amin, Vector2.zero) && Approx(amax, Vector2.one))
        {
            var pos = ReverseY(parent.anchoredPosition) + ReverseY(t.anchoredPosition);
            var size = t.rect.size;

            this.pos = pos;
            this.size = size;
            anchor = Vector2.zero;
        }
        else throw new Exception("Un Support Anchor");
    }
}
