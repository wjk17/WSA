using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MathTool
{
    /// <summary>
    /// 浮点数近似比较（用处近似于Mathf.Approximately，可调整精度）
    /// 保留dec位小数的近似比较（默认2位）
    /// </summary>
    /// <param name="f1">浮点数1</param>
    /// <param name="f2">浮点数2</param>
    /// <returns>返回是否近似</returns>
    public static bool Approx(this float f1, float f2)
    {
        return Approx(f1, f2, 2);
    }
    public static bool Approx(float f1, float f2, int dec)
    {
        float factor = Mathf.Pow(10, dec);
        return Approx(f1, f2, factor);
    }
    public static bool Approx(float f1, float f2, float factor)
    {
        //return
        //(Ceil(f1, factor) == Ceil(f2, factor)) ||
        //(Floor(f1, factor) == Ceil(f2, factor)) ||
        //(Floor(f1, factor) == Floor(f2, factor)) ||
        //(Ceil(f1, factor) == Floor(f2, factor));
        return Round(f1, factor) == Round(f2, factor);
    }
    public static float Round(float f, float factor)//进一
    {
        f *= factor;
        f = Mathf.Round(f);
        f /= factor;
        return f;
    }
    public static float Ceil(float f, float factor)//进一
    {
        f *= factor;
        f = Mathf.Ceil(f);
        f /= factor;
        return f;
    }
    public static float Floor(float f, float factor)//退一
    {
        f *= factor;
        f = Mathf.Floor(f);
        f /= factor;
        return f;
    }
    //public static float ApproxCeil(this float f)
    //{
    //    string dotLeft;
    //    string s =f.ToString();
    //    int os  = s.IndexOf('.');
    //    if (os > 0)
    //    {
    //        s = s.Substring(os + 1);
    //        os = s.IndexOf('9');
    //        s.Substring(os, 1);
    //        os
    //    }
    //}
    //public static Vector3 ApproxCeil(this Vector3 vector)
    //{
    //    vector.SetX(vector.x)
    //}
    /// <summary>
    /// 保留几位小数
    /// </summary>
    public static float Keep(float f, int fracCount)
    {
        float factor = Mathf.Pow(10, fracCount);
        float factor2 = 1 / factor;
        float v = Mathf.Round(f * factor) * factor2;
        return v;
    }
    public static string ToStr(this Vector3 v, int fracCount = 3, string splitStr = ", ")
    {
        return Keep(v.x, fracCount).ToString() +splitStr + 
            Keep(v.y, fracCount).ToString() + splitStr + 
            Keep(v.z, fracCount).ToString();
    }
    public static string ToStr(this Vector2 v, int fracCount = 3, string splitStr = ", ")
    {
        return Keep(v.x, fracCount).ToString() + splitStr +
            Keep(v.y, fracCount).ToString();
    }
}
