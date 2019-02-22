using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public static class Palette
    {
        public static Color pink { get { return new Color(0.8f, 0.1f, 0.1f); } }
        public static Color magenta { get { return HtmlToColor("E0257B"); } }
        public static Color darkBlue { get { return HtmlToColor("2A4180"); } }
        public const float v10 = 10f / 255f;
        public static readonly Color L10 = new Color(v10, v10, v10);
        public static readonly Color L1 = new Color(0.1f, 0.1f, 0.1f);
        public static readonly Color L2 = new Color(0.2f, 0.2f, 0.2f);
        public static readonly Color L3 = new Color(0.3f, 0.3f, 0.3f);
        public static readonly Color L4 = new Color(0.4f, 0.4f, 0.4f);
        public static readonly Color L5 = new Color(0.5f, 0.5f, 0.5f);
        public static readonly Color L6 = new Color(0.6f, 0.6f, 0.6f);
        public static readonly Color L7 = new Color(0.7f, 0.7f, 0.7f);
        public static readonly Color L8 = new Color(0.8f, 0.8f, 0.8f);
        public static readonly Color L9 = new Color(0.9f, 0.9f, 0.9f);
        public static Color HtmlToColor(string str)
        {
            var f = 1f / 255f;
            var r = str.Substring(0, 2).HexToInt() * f;
            var g = str.Substring(2, 2).HexToInt() * f;
            var b = str.Substring(4, 2).HexToInt() * f;
            return new Color(r, g, b);
        }
        public static Color Lerp(this Color c1, Color c2, float t)
        {
            return new Color(
                Mathf.Lerp(c1.r, c2.r, t),
                Mathf.Lerp(c1.g, c2.g, t),
                Mathf.Lerp(c1.b, c2.b, t),
                Mathf.Lerp(c1.a, c2.a, t));
        }

    }
}