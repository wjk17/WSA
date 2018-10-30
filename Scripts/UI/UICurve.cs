using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// TODO 做单选Toggle
/// </summary>
public partial class UICurve : MonoSingleton<UICurve>
{
    public Vector2 SIZE = Vector2.one;
    public void Start()
    {
        if (curve == null || curve.Count == 0)
            curve = new Curve2(Vector2.zero, Vector2.one * 0.5f + Vector2.up * 0.2f);
        //curve = new Curve2();
        UI.I.AddInputCB(name, GetInput, -2);
        OnResize();
        gridLinesCount.x = gridLinesCount.y * (rtSize.x / rtSize.y) * gridLinesXdivideY;
    }
    void OnResize()
    {
        var pos = rtPos / UI.scaler.referenceResolution;
        var scl = rtSize / UI.scaler.referenceResolution;
        matrixViewToRect = Matrix4x4.TRS(pos, Quaternion.identity, scl);
        pos = rtPos;
        scl = rtSize;
        // 将 规格化坐标（曲线空间） 转为屏幕坐标（Ref）
        var s = Vector2.one / SIZE / scl;
        matrixRectNorToScreenRef = Matrix4x4.TRS(pos, Quaternion.identity, s);
        // ↑ 的逆矩阵
        matrixScreenRefToRectNor = Matrix4x4.Scale(SIZE / scl) * Matrix4x4.Translate(-pos);
    }
    void GetInput()
    {
        if (!gameObject.activeSelf || !enabled) return;
        this.CheckResize(OnResize);

        if (Vector2.Distance(Input.mousePosition, prevPos) > moveError) { selIdx = 0; move = true; }

        mousePosRf = UI.mousePosRef_LB;
        mousePosRectN = matrixScreenRefToRectNor.MultiplyPoint(mousePosRf);

        pts = new List<Vector2>();
        var ks = new List<Key2>();
        var idx = new List<int>();
        foreach (var key in curve)
        {
            pts.Add(matrixRectNorToScreenRef.MultiplyPoint(key.vector));
            ks.Add(key);
            idx.Add(0);
            if (keySel != key) continue;
            if (key.inMode == KeyMode.Bezier)
            {
                pts.Add(matrixRectNorToScreenRef.MultiplyPoint(key.inTangent));
                ks.Add(key);
                idx.Add(1);
            }
            if (key.outMode == KeyMode.Bezier)
            {
                pts.Add(matrixRectNorToScreenRef.MultiplyPoint(key.outTangent));
                ks.Add(key);
                idx.Add(2);
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
                var rect = new Rt(pts[i], Vector2.one * sizeClick, Vectors.half);
                if (rect.Contains(mousePosRf))
                {
                    dragging = true;
                    keySel = ks[i];
                    if (idx[i] == 0)
                    {
                        subIdxs.Add(0);
                        selKeys.Add(ks[i]);
                        oss.Add(mousePosRectN - keySel.vector);
                    }
                    else if (idx[i] == 1)
                    {
                        subIdxs.Add(1);
                        selKeys.Add(ks[i]);
                        oss.Add(mousePosRectN - keySel.inTangent);
                    }
                    else if (idx[i] == 2)
                    {
                        subIdxs.Add(2);
                        selKeys.Add(ks[i]);
                        oss.Add(mousePosRectN - keySel.outTangent);
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
                    keySel.SetVector(-os + mousePosRectN);
                    curve.Sort();
                }
                else if (i == 1)
                    keySel.inTangent = -os + mousePosRectN;
                else if (i == 2)
                    keySel.outTangent = -os + mousePosRectN;
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
                    keySel.inTangent = mousePosRectN;
                }
            }
            else if (Events.KeyDown(KeyCode.T))
            {
                if (keySel != null)
                {
                    keySel.outMode = KeyMode.Bezier;
                    keySel.outTangent = mousePosRectN;
                }
            }
            else if (Events.KeyDown(KeyCode.I))
            {
                var k = new Key2();
                k.vector = mousePosRectN;
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
        var onWin = mousePosRectN.Between(Vector2.zero, SIZE);
        if (onWin && Events.Mouse1to3) Events.Use();
    }
}
