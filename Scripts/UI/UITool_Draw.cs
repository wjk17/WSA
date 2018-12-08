﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI
{
    public static partial class UITool // Draw
    {
        public static bool debug;
        public static void Draw(this MonoBehaviour mono)
        {
            Draw(mono, Color.black);
        }
        public static void Draw(this MonoBehaviour mono, Color color)
        {
            Draw(mono.transform as RectTransform, color);
        }
        public static void Draw(this RectTransform rt, Color color)
        {
            Draw(new Rect(rt), color);
        }
        public static void Draw(this Rect rt, Color color)
        {
            Draw(rt.ToPointsCWLT(), color);
        }
        public static void DrawSquare(Vector2 pos, float size, Color color)
        {
            Draw(ToPointsRect(pos, size), color);
        }
        public static List<Vector2> ToPointsRect(Vector2 pos, float sideLength)
        {
            var vs = new List<Vector2>();
            var size = Vectors.half2d * sideLength;
            vs.Add(pos - size);
            vs.Add(pos - size.ReverseX());
            vs.Add(pos + size);
            vs.Add(pos + size.ReverseX());
            return vs;
        }
        /// <summary>
        /// 从（列表的）头到尾绘制线段，首尾相连
        /// </summary>
        /// <param name="vs"></param>
        public static void Draw(this IList<Vector2> vs)
        {
            Draw(vs, Color.red);
        }
        public static void Draw(this IList<Vector2> vs, Color color)
        {
            var prev = vs[vs.Count - 1];
            for (int i = 0; i < vs.Count; i++)
            {
                DrawLine(prev, vs[i], color);
                prev = vs[i];
            }
        }
        public static void DrawLine(Vector2 a, Vector2 b, Color color)
        {
            if (debug)
                Debug.DrawLine(a, b, color);
            else
                GLUI.DrawLine(a, b, color, false);
        }
    }
}