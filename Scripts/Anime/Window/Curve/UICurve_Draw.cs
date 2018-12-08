using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI
{
    public partial class UICurve
    {
        public Color clrVectorSel = Palette.magenta;
        public Color clrVectorUnSel = Color.grey;

        public Color clrCtrlLinesSel = Palette.magenta;
        public Color clrCtrlLinesUnSel = Color.grey;

        public Color clrTrack = Palette.darkBlue;
        public Color clrSubSel = Color.red;

        public bool drawSel = true;
        public Vector2 gridFactor = new Vector2(0.2f, 1f);

        public float gridLinesXdivideY = 0.85f;
        public Color clrGridLinesFst = Palette.L10;
        public Color clrGridLinesSnd = Palette.L1;
        public Color clrBorder = Color.grey;

        public Vector2Int idx;
        private void Awake()
        {
            Curve2.drawLine = DrawLine;
            Curve2.drawVector = DrawVector;
            Curve2.drawTangent = DrawTangent;
            Curve2.colorVectors = Color.black;
            Curve2.colorTangents = Color.black;
        }
        private void Update()
        {
            this.FrameStart();

            Curve2.colorTrack = clrTrack;
            Curve2.colorCtrlLines = clrCtrlLinesUnSel;
            Curve2.colorBorder = clrBorder;

            // TODO GridLine 对应 Offset调整
            // TODO Offset 绘制Curve

            var lineSpaceFst = (drawAreaSize * gridFactor).RoundToInt();
            lineSpaceFst = Vector2Int.Max(Vector2Int.one * 2, lineSpaceFst);
            var lineSpaceSnd = (lineSpaceFst * Vectors.half2d).RoundToInt();
            idx = Vector2Int.zero;
            // grids
            var a = Vector2.zero;
            var b = drawAreaSize.X();
            for (int i = 0; i <= drawAreaSize_I.y; b.y = a.y = i += lineSpaceSnd.y)
            {
                DrawLine(a, b, idx.y++.IsEven() ? clrGridLinesFst : clrGridLinesSnd, m_Curve_Ref);
            }
            a = Vector2.zero;
            b = drawAreaSize.Y();
            for (int i = 0; i <= drawAreaSize.x; b.x = a.x = i += lineSpaceSnd.x)
            {
                DrawLine(a, b, idx.x++.IsEven() ? clrGridLinesFst : clrGridLinesSnd, m_Curve_Ref);
            }
            // timeline
            GLUI.BeginOrder(4);
            b.x = a.x = UITimeLine.I.frameIdx;
            DrawLine(a, b, UITimeLine.I.clrTimeLine, m_Curve_Ref);

            /// Curve
            if (curve == null || curve.Count == 0) return;
            Key2 k = keySel;
            if (mirror) // 使用默认方法画出曲线线段
            {
                curveMirror.drawAreaSize = drawAreaSize;
                curveMirror.Draw(m_Curve_Ref, showTangentsUnSel);
            }
            else
            {
                curve.drawAreaSize = drawAreaSize;
                curve.Draw(m_Curve_Ref, showTangentsUnSel);
            }

            GLUI.BeginOrder(3);
            var l = curve.Last().frameKey;
            var e = new Vector2(drawAreaSize.x, l.y);
            DrawLine(l, e, clrTrack, m_Curve_Ref);

            if (drawSel && k != null)
            {
                var i = (subIdxs != null && id < subIdxs.Count) ? subIdxs[id] : -1;


                if (k.inMode == KeyMode.Bezier)
                {
                    DrawLine(k.frameKey, k.inKey, i == 0 || i == 1 ? clrSubSel : clrCtrlLinesSel, m_Curve_Ref);
                    //DrawTangent(k.inTangent, Curve2.colorTangents, matrixViewToRect);
                }
                if (k.outMode == KeyMode.Bezier && !(mirror && k.time == 0.5f))
                {
                    DrawLine(k.frameKey, k.outKey, i == 0 || i == 2 ? clrSubSel : clrCtrlLinesSel, m_Curve_Ref);
                    //DrawTangent(k.outTangent, Curve2.colorTangents, matrixViewToRect);
                }

                DrawRect(k.frameKey, Vector2.one * sizeDrawVector,
                    Curve2.colorVectors, m_Curve_Ref, i == 0 ? clrSubSel : clrVectorSel, true);
                if (k.inMode == KeyMode.Bezier)
                {
                    DrawRhombus(k.inKey, Vector2.one * sizeDrawTangent,
                        Curve2.colorTangents, m_Curve_Ref, i == 0 || i == 1 ? clrSubSel : clrCtrlLinesSel, true);
                }
                if (k.outMode == KeyMode.Bezier && !(mirror && k.time == 0.5f))
                {
                    DrawRhombus(k.outKey, Vector2.one * sizeDrawTangent,
                        Curve2.colorTangents, m_Curve_Ref, i == 0 || i == 2 ? clrSubSel : clrCtrlLinesSel, true);
                }
            }
        }
        private void DrawTangent(Vector2 p, Color color, Matrix4x4 m)
        {
            DrawRhombus(p, Vector2.one * sizeDrawTangent, color, m, clrVectorUnSel, true);
        }
        void DrawRhombus(Vector2 p, Vector2 size, Color color, Matrix4x4 m, Color solidColor, bool solid = false)
        {
            size = m_Ref_Curve.ScaleV2(size);
            var t = p + Vector2.up * size * 0.5f;
            var b = p - Vector2.up * size * 0.5f;
            var l = p + Vector2.right * size * 0.5f;
            var r = p - Vector2.right * size * 0.5f;

            if (solid)
            {
                DrawQuads(t, l, b, r, solidColor, m);
            }
            //else
            {
                DrawLine(t, l, color, m);
                DrawLine(l, b, color, m);
                DrawLine(b, r, color, m);
                DrawLine(r, t, color, m);
            }
        }
        void DrawVector(Vector2 p, Color color, Matrix4x4 m)
        {
            DrawRect(p, Vector2.one * sizeDrawVector, color, m, clrVectorUnSel, true);
        }
        void DrawRect(Vector2 p, Vector2 size, Color color, Matrix4x4 m, Color solidColor, bool solid = false)
        {
            size = m_Ref_Curve.ScaleV2(size);
            var lt = p - size * 0.5f;
            lt.y += size.y; // 左上和左下坐标的转换
            var lb = p - size * 0.5f;
            var rb = p + size * 0.5f;
            rb.y -= size.y; // 其实也没必要，本来这么想但如果注释顺序就错了
            var rt = p + size * 0.5f;

            if (solid)
            {
                DrawQuads(lt, lb, rb, rt, solidColor, m);
            }
            //else
            {
                DrawLine(lt, rt, color, m);
                DrawLine(lb, rb, color, m);
                DrawLine(lt, lb, color, m);
                DrawLine(rt, rb, color, m);
            }
        }
        void DrawQuads(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color color, Matrix4x4 m)
        {
            a = m.MultiplyPoint(a);
            b = m.MultiplyPoint(b);
            c = m.MultiplyPoint(c);
            d = m.MultiplyPoint(d);
            GLUI.DrawQuads(a, b, c, d, color);
        }
        void DrawLine(Vector2 a, Vector2 b, Color color, Matrix4x4 m) // 接口 
        {
            a = m.MultiplyPoint(a);
            b = m.MultiplyPoint(b);
            GLUI.DrawLine(a, b, color);
        }
    }
}