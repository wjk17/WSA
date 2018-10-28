using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Curve2
{
    public delegate void ActDrawLine(Vector2 a, Vector2 b, Color color, Matrix4x4 m);
    public delegate void ActDrawPoint(Vector2 p, Color color, Matrix4x4 m);
    public static ActDrawLine drawLine;
    public static ActDrawPoint drawVector;
    public static ActDrawPoint drawTangent;
    public static Color colorTrack;
    public static Color colorCtrlLines;
    public static Color colorVectors;
    public static Color colorTangents;
    public static Color colorBorder;
    public static int accuracy = 100;
    internal void Draw(Matrix4x4 m, bool showTangents)
    {
        DrawBorder(m);
        DrawLines(m, showTangents);
        DrawPoints(m, showTangents);
    }

    private void DrawBorder(Matrix4x4 m)
    {
        var lt = new Vector2(0, 1);
        var lb = Vector2.zero;
        var rb = new Vector2(1, 0);
        var rt = Vector2.one;
        drawLine(lt, rt, colorBorder, m);
        drawLine(lt, lb, colorBorder, m);
        drawLine(rt, rb, colorBorder, m);
        drawLine(lb, rb, colorBorder, m);
    }

    public void DrawPoints(Matrix4x4 m, bool showTangents)
    {
        foreach (var key in keys)
        {
            drawVector(key.vector, colorVectors, m);
            if (showTangents && key.inMode == KeyMode.Bezier) drawTangent(key.inTangent, colorTangents, m);
            if (showTangents && key.outMode == KeyMode.Bezier) drawTangent(key.outTangent, colorTangents, m);
        }
    }
    public void DrawLines(Matrix4x4 m, bool showTangents)
    {
        if (keys == null || Count == 0) return;

        var factor = 1f / (accuracy - 1);
        var prev = new Vector2(0f, keys[0].value);
        float t;
        Vector2 curr;
        for (int i = 1; i < accuracy; i++)
        {
            t = i * factor;
            if (time1D)
            {
                curr = new Vector2(t, Evaluate1D(i * factor));
            }
            else
            {
                curr = Evaluate2D(i * factor);
            }
            drawLine(prev, curr, colorTrack, m);
            prev = curr;
        }

        foreach (var key in keys)
        {
            if (showTangents && key.inMode == KeyMode.Bezier) drawLine(key.vector, key.inTangent, colorCtrlLines, m);
            if (showTangents && key.outMode == KeyMode.Bezier) drawLine(key.vector, key.outTangent, colorCtrlLines, m);
        }
    }
}
