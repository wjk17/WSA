using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Rt
{
    public Vector2 p;
    public Vector2 size;
    public Rt(Vector2 p, Vector2 size)
    {
        this.p = p;
        this.size = size;
    }
    public bool Contains(Vector2 v)
    {
        var lt = p - size * 0.5f;
        var rb = p + size * 0.5f;
        return MathTool.Between(v, lt, rb);
    }
}
