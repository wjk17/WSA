using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// TODO不再使用graphics.DrawMesh而是GL画线，不再使用3D碰撞体而是2D点击检测
/// </summary>
public partial class Curve3
{
    public delegate void ActDrawLine(Vector3 a, Vector3 b, Color color);
    public delegate void ActDrawPoint(Vector3 a, Color color);
    public static ActDrawLine drawLine;
    public static ActDrawPoint drawPoint;
    public static Color colorTrack;
    public static Color colorControlLines;
    public static Color colorPoints;
    public static int accuracy = 100;
    internal void Draw()
    {
        DrawPoints();
        DrawLines();
    }
    public void DrawPoints()
    {
        foreach (var key in keys)
        {
            drawPoint(key.vector, colorPoints);
            //if (key.inMode == KeyMode.Bezier) drawPoint(key.inTangent, colorPoints);
            //if (key.outMode == KeyMode.Bezier) drawPoint(key.outTangent, colorPoints);
        }
    }
    public void DrawLines()
    {
        foreach (var key in keys)
        {
            //if (key.inMode == KeyMode.Bezier) drawLine(key.vector, key.inTangent, colorControlLines);
            //if (key.outMode == KeyMode.Bezier) drawLine(key.vector, key.outTangent, colorControlLines);
        }
        if (keys == null || Count == 0) return;
        var factor = 1f / (accuracy - 1);
        var prev = keys[0].vector;
        for (int i = 1; i < accuracy; i++)
        {
            var curr = Evaluate(i * factor);
            drawLine(prev, curr, colorTrack);
            prev = curr;
        }
    }
}
