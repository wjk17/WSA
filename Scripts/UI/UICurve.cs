using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
=======
//using Esa;
>>>>>>> 36ecf3a9dfc01741cc93e9b0c92d2ca525d75f9d
namespace Esa.UI
{
    /// <summary>
    /// TODO 做单选Toggle
    /// </summary>
<<<<<<< HEAD
    public partial class UICurve : MonoSingleton<UICurve>
=======
    public partial class UICurve : Singleton<UICurve>
>>>>>>> 36ecf3a9dfc01741cc93e9b0c92d2ca525d75f9d
    {
        public Vector2 drawAreaSize = Vector2.one;
        public Vector2 drawAreaOffset = Vector2.zero;
        Vector2 _drawAreaSize;
        Vector2 _drawAreaOffset;
        public void Start()
        {
            curve = new Curve2(Vector2.zero, drawAreaSize * 0.5f + drawAreaSize * Vector2.up * 0.2f);
            this.AddInputCB(GetInput, -2);
            OnResize();
        }
        void OnResize()
        {
            _drawAreaSize = drawAreaSize; // Curve空间
            _drawAreaOffset = drawAreaOffset; // Ref空间
            var pos = (rtPos + drawAreaOffset) / UI.scaler.referenceResolution;
            //var pos = rtPos / UI.scaler.referenceResolution;
            var scl = rtSize / UI.scaler.referenceResolution / drawAreaSize;
            m_Curve_V = Matrix4x4.TRS(pos, Quaternion.identity, scl);
            pos = rtPos;

            // 将 规格化坐标（曲线空间） 转为屏幕坐标（Ref）
            scl = drawAreaSize / rtSize;
            m_Curve_Ref = Matrix4x4.TRS(pos, Quaternion.identity, scl);
            // ↑ 的逆矩阵
            m_Ref_Curve = Matrix4x4.Scale(scl) * Matrix4x4.Translate(-pos);
        }
        void GetInput()
        {
            if (!gameObject.activeSelf || !enabled) return;
            this.CheckResize(OnResize);
            if (_drawAreaSize != drawAreaSize || _drawAreaOffset != drawAreaOffset) OnResize();

            if (Vector2.Distance(Input.mousePosition, prevPos) > moveError) { selIdx = 0; move = true; }

            mousePosRef = UI.mousePosRef_LB;
            mousePosCurve = m_Ref_Curve.MultiplyPoint(mousePosRef);
            var sizeClickCurve = m_Ref_Curve.ScaleV2(Vector2.one * sizeClick);

            pts = new List<Vector2>(); // 可以点击的位置
            var ks = new List<Key2>();
            var idx = new List<int>();
            foreach (var key in curve)
            {
                pts.Add(key.vector);
                ks.Add(key); idx.Add(0);
                if (keySel == key) // 如果选中的点有切点，这时才显示出来让用户点击
                {
                    if (key.inMode == KeyMode.Bezier)
                    {
                        pts.Add(key.inTangent);
                        ks.Add(key); idx.Add(1);
                    }
                    if (key.outMode == KeyMode.Bezier)
                    {
                        pts.Add(key.outTangent);
                        ks.Add(key); idx.Add(2);
                    }
                }
            }
            if (Events.MouseDown(MB.Right))
            {
                selKeys = new List<Key2>();
                subIdxs = new List<int>();
                oss = new List<Vector2>();
                bool click = false; ;
                for (int i = 0; i < pts.Count; i++)
                {
                    var rect = new Rt(pts[i], sizeClickCurve, Vectors.half);
                    if (rect.Contains(mousePosCurve))
                    {
                        dragging = true;
                        keySel = ks[i];
                        if (idx[i] == 0)
                        {
                            subIdxs.Add(0); selKeys.Add(ks[i]);
                            oss.Add(mousePosCurve - keySel.vector);
                        }
                        else if (idx[i] == 1)
                        {
                            subIdxs.Add(1); selKeys.Add(ks[i]);
                            oss.Add(mousePosCurve - keySel.inTangent);
                        }
                        else if (idx[i] == 2)
                        {
                            subIdxs.Add(2); selKeys.Add(ks[i]);
                            oss.Add(mousePosCurve - keySel.outTangent);
                        }
                        else throw new System.Exception();
                        if (!click && !move)
                        {
                            click = true;
                            selIdx++; // 如果连续点击同一位置两次，允许选择同位置可以点击到的其他锚点
                        }
                    }
                }
                move = false;
                if (selKeys.Count > 0)
                {
                    id = selIdx % selKeys.Count;
                    keySel = selKeys[id];
                    os = oss[id];
                }
            }
            else if (Events.Mouse(MB.Right))
            {
                if (dragging)
                {
                    var i = subIdxs[id];
                    if (i == 0)
                    {
                        keySel.SetVector(-os + mousePosCurve);
                        curve.Sort();
                    }
                    else if (i == 1)
                        keySel.inTangent = -os + mousePosCurve;
                    else if (i == 2)
                        keySel.outTangent = -os + mousePosCurve;
                }
            }
            else if (Events.MouseDown(MB.Middle))
            {
                if (mousePosCurve.Between(Vector2.zero, drawAreaSize))
                {
                    dragging = true;
                    os = mousePosRef - drawAreaOffset;
                }
            }
            else if (Events.Mouse(MB.Middle))
            {
                if (dragging)
                {
                    drawAreaOffset = mousePosRef - os;
                }
            }
            else
            {
                dragging = false;
                if (Events.KeyDown(KeyCode.R))
                {
                    if (keySel != null)
                    {
                        keySel.inMode = KeyMode.Bezier;
                        keySel.inTangent = mousePosCurve;
                    }
                }
                else if (Events.KeyDown(KeyCode.T))
                {
                    if (keySel != null)
                    {
                        keySel.outMode = KeyMode.Bezier;
                        keySel.outTangent = mousePosCurve;
                    }
                }
                else if (Events.KeyDown(KeyCode.I)) // 插入关键帧
                {
                    var k = new Key2();
                    k.vector = mousePosCurve;
                    curve.InsertKey(k);
                }
                else if (Events.KeyDown(KeyCode.X))
                {
                    if (keySel != null)
                    {
                        var i = subIdxs[id];
                        if (i == 0)
                        {
                            curve.Remove(keySel);
                            keySel = null;
                            curve.Sort();
                        }
                        else if (i == 1)
                        {
                            keySel.inMode = KeyMode.None;
                            keySel = null;
                            //subIdxs.RemoveAt(id);
                        }
                        else if (i == 2)
                        {
                            keySel.outMode = KeyMode.None;
                            keySel = null;
                            //subIdxs.RemoveAt(id);
                        }
                    }
                }
            }
            if (dragging) Events.Use(); // 一旦Use连Key和Mouse方法都检测不到的
            prevPos = Input.mousePosition;
            if (mirror)
            {
                curveMirror = curve.Clone();
                curveMirror.Mirror(mirrorError, curve);
            }
            var onWin = mousePosCurve.Between(Vector2.zero, drawAreaSize);
            if (onWin && Events.Mouse1to3) Events.Use();
        }
    }
}