using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class Palette
{
    public static Color pink { get { return new Color(0.8f, 0.1f, 0.1f); } }
    public static Color magenta { get { return HtmlToColor("E0257B"); } }
    public static Color darkBlue { get { return HtmlToColor("2A4180"); } }
    public const float v10 = 10f / 255f;
    public static readonly Color L10 = new Color(v10, v10, v10);
    public static readonly Color L1 = new Color(0.1f, 0.1f, 0.1f);
    public static Color HtmlToColor(string str)
    {
        var f = 1f / 255f;
        var r = str.Substring(0, 2).HexToInt() * f;
        var g = str.Substring(2, 2).HexToInt() * f;
        var b = str.Substring(4, 2).HexToInt() * f;
        return new Color(r, g, b);
    }
}

