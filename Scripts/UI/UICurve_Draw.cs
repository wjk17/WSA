using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UICurve
{
    public Color clrVectorSel;
    public Color clrVectorUnSel = Color.grey;

    public Color clrCtrlLinesSel;
    public Color clrCtrlLinesUnSel = Color.grey;

    public Color clrTrack = Color.green;
    public Color clrSubSel = Color.red;
    private void Awake()
    {
        Curve2.drawLine = DrawLine;
        Curve2.drawVector = DrawVector;
        Curve2.drawTangent = DrawTangent;
        Curve2.colorVectors = Color.black;
        Curve2.colorTangents = Color.black;
    }
    private void DrawTangent(Vector2 p, Color color, Matrix4x4 m)
    {
        DrawRhombus(p, Vector2.one * sizeDrawTangent, color, m, clrVectorUnSel, true);
    }
    void DrawRhombus(Vector2 p, Vector2 size, Color color, Matrix4x4 m, Color solidColor, bool solid = false)
    {
        size /= rtSize; // to N
        var t = p + Vector2.up * size * 0.5f;
        var b = p - Vector2.up * size * 0.5f;
        var l = p + Vector2.right * size * 0.5f;
        var r = p - Vector2.right * size * 0.5f;

        if (solid)
        {
            DrawLines.DrawQuads(solidColor, new Vector2[] { t, l, b, r }, m);
        }
        //else
        {
            DrawLine(t, l, color, m);
            DrawLine(l, b, color, m);
            DrawLine(b, r, color, m);
            DrawLine(r, t, color, m);
        }
    }
    public bool drawSel;
    public Vector2 gridLinesCount = new Vector2(15, 10);
    public Color clrGridLines = Color.grey;
    public Color clrBorder = Color.grey;
    private void OnRenderObject()
    {
        Curve2.colorTrack = clrTrack;
        Curve2.colorCtrlLines = clrCtrlLinesUnSel;
        Curve2.colorBorder = clrBorder;

        // grids
        var a = Vector2.zero;
        var b = Vector2.right;
        var factor = 1f / gridLinesCount.y;
        for (int i = 0; i < gridLinesCount.y; i++)
        {
            b.y = a.y = i * factor;
            DrawLine(a, b, clrGridLines, matrixViewToRect);
        }
        a = Vector2.zero;
        b = Vector2.up;
        factor = 1f / gridLinesCount.x;
        for (int i = 0; i < gridLinesCount.x; i++)
        {
            b.x = a.x = i * factor;
            DrawLine(a, b, clrGridLines, matrixViewToRect);
        }

        if (curve == null || curve.Count == 0) return;

        Key2 k = keySel;
        if (mirror)
            curveMirror.Draw(matrixViewToRect, showTangentsUnSel);
        else
            curve.Draw(matrixViewToRect, showTangentsUnSel);

        if (drawSel && k != null)
        {
            var i = (subIdxs != null && id < subIdxs.Count) ? subIdxs[id] : -1;


            if (k.inMode == KeyMode.Bezier)
            {
                DrawLine(k.vector, k.inTangent, i == 0 || i == 1 ? clrSubSel : clrCtrlLinesSel, matrixViewToRect);
                //DrawTangent(k.inTangent, Curve2.colorTangents, matrixViewToRect);
            }
            if (k.outMode == KeyMode.Bezier && !(mirror && k.time == 0.5f))
            {
                DrawLine(k.vector, k.outTangent, i == 0 || i == 2 ? clrSubSel : clrCtrlLinesSel, matrixViewToRect);
                //DrawTangent(k.outTangent, Curve2.colorTangents, matrixViewToRect);
            }

            DrawRect(k.vector, Vector2.one * sizeDrawVector,
                Curve2.colorVectors, matrixViewToRect, i == 0 ? clrSubSel : clrVectorSel, true);
            if (k.inMode == KeyMode.Bezier)
            {
                DrawRhombus(k.inTangent, Vector2.one * sizeDrawTangent,
                    Curve2.colorTangents, matrixViewToRect, i == 0 || i == 1 ? clrSubSel : clrCtrlLinesSel, true);
            }
            if (k.outMode == KeyMode.Bezier && !(mirror && k.time == 0.5f))
            {
                DrawRhombus(k.outTangent, Vector2.one * sizeDrawTangent,
                    Curve2.colorTangents, matrixViewToRect, i == 0 || i == 2 ? clrSubSel : clrCtrlLinesSel, true);
            }
        }
    }
    void DrawRect(Vector2 p, Vector2 size, Color color, Matrix4x4 m, Color solidColor, bool solid = false)
    {
        size /= rtSize; // to N
        var lt = p - size * 0.5f;
        lt.y += size.y; // 左上和左下坐标的转换
        var lb = p - size * 0.5f;
        var rb = p + size * 0.5f;
        rb.y -= size.y; // 其实也没必要，本来这么想但如果注释顺序就错了
        var rt = p + size * 0.5f;

        if (solid)
        {
            DrawLines.DrawQuads(solidColor, new Vector2[] { lt, lb, rb, rt }, m);
        }
        //else
        {
            DrawLine(lt, rt, color, m);
            DrawLine(lb, rb, color, m);
            DrawLine(lt, lb, color, m);
            DrawLine(rt, rb, color, m);
        }
    }
    void DrawVector(Vector2 p, Color color, Matrix4x4 m)
    {
        DrawRect(p, Vector2.one * sizeDrawVector, color, m, clrVectorUnSel, true);
    }
    void DrawLine(Vector2 a, Vector2 b, Color color, Matrix4x4 m) // 接口 
    {
        DrawLines.DoDrawLines(color, new Vector2[] { a, b }, new int[] { 0, 1 }, m);
    }
}
