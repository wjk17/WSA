using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MatrixTool
{
    public static List<float> ToList(this Matrix4x4 m)
    {
        var list = new List<float>();
        for (int i = 0; i < 16; i++)
        {
            list.Add(m[i]);
        }
        return list;
    }
    public static float ScaleX(this Matrix4x4 m, float f)
    {
        return m.m00 * f;
    }
    public static float ScaleY(this Matrix4x4 m, float f)
    {
        return m.m11 * f;
    }
    public static float ScaleZ(this Matrix4x4 m, float f)
    {
        return m.m22 * f;
    }
    public static Vector2 ScaleV2(this Matrix4x4 m, Vector2 f)
    {
        return new Vector2(f.x * m.m00, f.y * m.m11);
    }
    public static Vector3 ScaleV3(this Matrix4x4 m, Vector3 f)
    {
        return new Vector3(f.x * m.m00, f.y * m.m11, f.z * m.m22);
    }
}
