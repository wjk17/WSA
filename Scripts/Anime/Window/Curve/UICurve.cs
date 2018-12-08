﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI
{
    /// <summary>
    /// TODO 做单选Toggle
    /// </summary>
    public partial class UICurve : Singleton<UICurve>
    {
        public Vector2Int drawAreaSize_I { get { return drawAreaSize.RoundToInt(); } }
        public Vector2 drawAreaSize = Vector2.one;
        Vector2 _drawAreaSize;
        public Vector2Int drawAreaOffset_I { get { return drawAreaOffset.RoundToInt(); } }
        public Vector2 drawAreaOffset = Vector2.zero;
        Vector2 _drawAreaOffset;
        public void Start()
        {
            curve = new Curve2(Vector2.zero, drawAreaSize * 0.5f + drawAreaSize * Vector2.up * 0.2f);
            this.AddInput(GetInput, -2, false);
            OnResize();
        }
        void OnResize()
        {
            _drawAreaSize = drawAreaSize; // Curve空间
            _drawAreaOffset = drawAreaOffset; // Ref空间

            var pos = rtPos + drawAreaOffset;
            var scl = rtSize / drawAreaSize;

            // 将 规格化坐标（曲线空间） 转为画布坐标（Ref）
            m_Curve_Ref = Matrix4x4.TRS(pos, Quaternion.identity, scl);
            // ↑ 的逆矩阵
            m_Ref_Curve = Matrix4x4.Scale(Vector2.one / scl) * Matrix4x4.Translate(-pos);
        }
        private void LateUpdate()
        {
            drawAreaSize.x = UITimeLine.I.SIZE.x;
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
                        pts.Add(key.inTan);
                        ks.Add(key); idx.Add(1);
                    }
                    if (key.outMode == KeyMode.Bezier)
                    {
                        pts.Add(key.outTan);
                        ks.Add(key); idx.Add(2);
                    }
                }
            }
            if (Events.MouseDown0 && UI.MouseOver(rt, UITimeLine.I.rtArea))
            {
                dragging = true;
            }
            if (Events.Mouse0 && dragging)
            {
                UITimeLine.I.frameIdx_F = mousePosCurve.x;
            }
            else if (Events.MouseDown1)
            {
                selKeys = new List<Key2>();
                subIdxs = new List<int>();
                oss = new List<Vector2>();
                bool click = false; ;
                for (int i = 0; i < pts.Count; i++)
                {
                    var rect = new Rect(pts[i], sizeClickCurve);
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
                            oss.Add(mousePosCurve - keySel.inTan);
                        }
                        else if (idx[i] == 2)
                        {
                            subIdxs.Add(2); selKeys.Add(ks[i]);
                            oss.Add(mousePosCurve - keySel.outTan);
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
            else if (Events.Mouse1)
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
                        keySel.inTan = -os + mousePosCurve;
                    else if (i == 2)
                        keySel.outTan = -os + mousePosCurve;
                }
            }
            else if (Events.MouseDown2)
            {
                if (mousePosCurve.Between(Vector2.zero, drawAreaSize))
                {
                    dragging = true;
                    os = mousePosRef - drawAreaOffset;
                }
            }
            else if (Events.Mouse2)
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
                        keySel.inTan = mousePosCurve;
                    }
                }
                else if (Events.KeyDown(KeyCode.T))
                {
                    if (keySel != null)
                    {
                        keySel.outMode = KeyMode.Bezier;
                        keySel.outTan = mousePosCurve;
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