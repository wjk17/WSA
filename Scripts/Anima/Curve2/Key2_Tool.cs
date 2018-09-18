using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Key2_Tool {
    public static Vector2 LocalIn(this Key2 k)
    {
        return (k.inTangent - k.vector);
    }
    public static Vector2 SetLocalIn(this Key2 k, Vector2 v)
    {
        return k.inTangent = k.vector + v;
    }
    public static Vector2 LocalOut(this Key2 k)
    {
        return (k.outTangent - k.vector);
    }
    public static Vector2 SetLocalOut(this Key2 k, Vector2 v)
    {
        return k.outTangent = k.vector + v;
    }
    public static void SetLengthOut(this Key2 k, float len)
    {
        var dir = (k.outTangent - k.vector);
        if (dir != Vector2.zero)
            dir = dir.normalized;
        k.outTangent = k.vector + dir * len;
    }
    public static void SetLengthIn(this Key2 k, float len)
    {
        var dir = (k.inTangent - k.vector);
        if (dir != Vector2.zero)
            dir = dir.normalized;
        k.inTangent = k.vector + dir * len;
    }
}
