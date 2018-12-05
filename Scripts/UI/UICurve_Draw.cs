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
        public Vector2Int gridCount = new Vector2Int(16, 9);
        public Vector2 lineSpaceFst;
        public Vector2 lineSpaceSnd;

        public float gridLinesXdivideY = 0.85f;
        public Color clrGridLinesFst = Palette.L10;
        public Color clrGridLinesSnd = Palette.L1;
        public Color clrBorder = Color.grey;

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
            //简单裁剪
            if (!p.Between(drawAreaOffset, drawAreaOffset + drawAreaSize)) return;

            size = m_Ref_Curve.ScaleV2(size); // to CurveSpace
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
        public AnimationCurve curve11;
        public Vector2Int gridCountReal;
        public int reactDiffCount;
        public float minusSpace = 0.05f;//0.0001f
        private void OnRenderObject()
        {
            Curve2.colorTrack = clrTrack;
            Curve2.colorCtrlLines = clrCtrlLinesUnSel;
            Curve2.colorBorder = clrBorder;

            // TODO 使间隔平滑细分变细，定义整数调整（暂时用2倍和0.5f倍调整）
            // TODO 使用前版UI裁剪，合并后版的DoDrawline。
            // TODO GridLine 对应 Offset调整
            // TODO Offset 绘制Curve
            if (Mathf.Abs(gridCountReal.x - (gridCount.x * 2 + 1)) > reactDiffCount)
            {
                lineSpaceFst.x *= gridCountReal.x > gridCount.x ? 2 : 0.5f;
            }
            if (Mathf.Abs(gridCountReal.y - (gridCount.y * 2 + 1)) > reactDiffCount)
            {
                lineSpaceFst.y *= gridCountReal.y > gridCount.y ? 2 : 0.5f;
            }

            lineSpaceFst = Vector2.Max(lineSpaceFst, Vector2.one * minusSpace);
            lineSpaceSnd = lineSpaceFst * 0.5f;
            gridCountReal = Vector2Int.zero;
            // grids
            var a = Vector2.zero;
            var b = drawAreaSize.ToX0();
            for (float f = 0; f <= drawAreaSize.y; b.y = a.y = f += lineSpaceSnd.y)
            {
                DrawLine(a, b, gridCountReal.y++.IsEven() ? clrGridLinesFst : clrGridLinesSnd, m_Curve_V);
            }
            a = Vector2.zero;
            b = drawAreaSize.To0Y();
            for (float f = 0; f <= drawAreaSize.x; b.x = a.x = f += lineSpaceSnd.x)
            {
                DrawLine(a, b, gridCountReal.x++.IsEven() ? clrGridLinesFst : clrGridLinesSnd, m_Curve_V);
            }
            // timeline
            b.x = a.x = UITimeLine.I.frameIdx;
            DrawLine(a, b, UITimeLine.I.clrTimeLine, m_Curve_V);

            if (curve == null || curve.Count == 0) return;

            Key2 k = keySel;
            if (mirror) // 使用默认方法画出曲线线段
            {
                curveMirror.drawAreaSize = drawAreaSize;
                curveMirror.Draw(m_Curve_V, showTangentsUnSel);
            }
            else
            {
                curve.drawAreaSize = drawAreaSize;
                curve.Draw(m_Curve_V, showTangentsUnSel);
            }

            var l = curve.Last().vector;
            var e = new Vector2(drawAreaSize.x, l.y);
            DrawLine(l, e, clrTrack, m_Curve_V);

            if (drawSel && k != null)
            {
                var i = (subIdxs != null && id < subIdxs.Count) ? subIdxs[id] : -1;


                if (k.inMode == KeyMode.Bezier)
                {
                    DrawLine(k.vector, k.inTangent, i == 0 || i == 1 ? clrSubSel : clrCtrlLinesSel, m_Curve_V);
                    //DrawTangent(k.inTangent, Curve2.colorTangents, matrixViewToRect);
                }
                if (k.outMode == KeyMode.Bezier && !(mirror && k.time == 0.5f))
                {
                    DrawLine(k.vector, k.outTangent, i == 0 || i == 2 ? clrSubSel : clrCtrlLinesSel, m_Curve_V);
                    //DrawTangent(k.outTangent, Curve2.colorTangents, matrixViewToRect);
                }

                DrawRect(k.vector, Vector2.one * sizeDrawVector,
                    Curve2.colorVectors, m_Curve_V, i == 0 ? clrSubSel : clrVectorSel, true);
                if (k.inMode == KeyMode.Bezier)
                {
                    DrawRhombus(k.inTangent, Vector2.one * sizeDrawTangent,
                        Curve2.colorTangents, m_Curve_V, i == 0 || i == 1 ? clrSubSel : clrCtrlLinesSel, true);
                }
                if (k.outMode == KeyMode.Bezier && !(mirror && k.time == 0.5f))
                {
                    DrawRhombus(k.outTangent, Vector2.one * sizeDrawTangent,
                        Curve2.colorTangents, m_Curve_V, i == 0 || i == 2 ? clrSubSel : clrCtrlLinesSel, true);
                }
            }
        }
        void DrawRect(Vector2 p, Vector2 size, Color color, Matrix4x4 m, Color solidColor, bool solid = false)
        {
            //简单裁剪
            if (!p.Between(drawAreaOffset, drawAreaOffset + drawAreaSize)) return;

            size = m_Ref_Curve.ScaleV2(size); // to CurveSpace
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
        void DrawLine(Vector2 a, Vector2 b, Color color) // 接口 
        {
            DrawLines.DoDrawLines(color, new Vector2[] { a, b }, new int[] { 0, 1 });
        }
        void DrawLine(Vector2 a, Vector2 b, Color color, Matrix4x4 m) // 接口 
        {
            var scl = Vector2.one;// / UI.scaler.referenceResolution;
                                  //a -= drawAreaOffset * scl;
                                  //b -= drawAreaOffset * scl;
            DrawLines.DoDrawLines(color, new Vector2[] { a, b }, new int[] { 0, 1 }, m);
        }
    }
}