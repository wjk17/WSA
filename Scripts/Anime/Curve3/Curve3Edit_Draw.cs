using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Curve3Edit
{
    public int accuracy = 100; // 曲线精度

    public Color clrSubSel = Color.cyan;
    public Color clrPointsSel = Color.grey;//4E6E8E
    public Color clrPointsUnSel = Color.black;
    public float pointSize = 13;

    void Awake()
    {
        Curve3.drawLine = DrawLine;
        Curve3.drawPoint = DrawPoint;
        Curve3.colorControlLines = Color.black;
        Curve3.colorPoints = Color.black;
        Curve3.colorTrack = Color.black;
    }
    void DrawRect(Vector2 p, Vector2 size, Color color, Matrix4x4 m)
    {
        size /= UI.scaler.referenceResolution; // to N
        var lt = p - size * 0.5f;
        lt.y += size.y; // 左上和左下坐标的转换
        var lb = p - size * 0.5f;
        var rb = p + size * 0.5f;
        rb.y -= size.y; // 其实也没必要，本来这么想但如果注释顺序就错了
        var rt = p + size * 0.5f;

        DrawLines.DrawQuads(color, new Vector2[] { lt, lb, rb, rt }, m);

    }
    void DrawPoint(Vector3 p, Color color)
    {
        var m = Matrix4x4.identity;
        var p2 = Camera.main.WorldToViewportPoint(p);
        DrawRect(p2, pointSize * Vector2.one, color, m);
    }
    void DrawLine(Vector3 a, Vector3 b, Color color) // 接口 
    {
        a = Camera.main.WorldToViewportPoint(a);
        b = Camera.main.WorldToViewportPoint(b);

        DrawLines.DoDrawLines(color, new Vector2[] { a, b }, new int[] { 0, 1 }, Matrix4x4.identity);
    }
    void OnRenderObject()
    {
        if (editCurve)
        {
            Curve3.accuracy = accuracy;
            if (useSelector)
            {
                var hair = Selector.current.GetComponent<Hair>();
                hair.curveData.curve.Draw();
            }
            else curve.Draw();

            var k = keySelected;
            var i = idxSelected;
            if (k != null)
            {
                if (k.inMode == KeyMode.Bezier)
                {
                    DrawLine(k.vector, k.inTangent, i == 0 || i == 1 ? clrSubSel : clrPointsSel);
                }
                if (k.outMode == KeyMode.Bezier)
                {
                    DrawLine(k.vector, k.outTangent, i == 0 || i == 2 ? clrSubSel : clrPointsSel);
                }

                var p = k.vector;
                DrawPoint(p, i == 0 ? clrSubSel : clrPointsSel);

                if (k.inMode == KeyMode.Bezier)
                {
                    p = k.inTangent;
                    DrawPoint(p, i == 0 || i == 1 ? clrSubSel : clrPointsSel);
                }
                if (k.outMode == KeyMode.Bezier)
                {
                    p = k.outTangent;
                    DrawPoint(p, i == 0 || i == 2 ? clrSubSel : clrPointsSel);
                }
            }
        }
    }
}
