using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TODO 做单选Toggle
/// </summary>
public partial class UICurve : MonoSingleton<UICurve>
{
    [Range(0, 5)]
    public int curveIdx;
    int _curveIdx_pre;
    public Text curveIdx_text;    
    void GetCurve()
    {
        var oc = UIClip.I.clip.GetCurve(UIDOFEditor.I.ast);
        curve = oc == null ? null : oc.curves[curveIdx];
    }
    public void Start()
    {
        UpdatePoss();
        this.AddInputCB(GetInput, -2);
    }
    //[CheckMove]// TODO 编辑器环境下自动收集
    //[MAD.ShowProperty(MAD.ShowPropertyAttribute.EValueType.Vector2)]
    public Vector2 nSize { set { _nSize = value; UpdatePoss(); } get { return _nSize; } }
    [SerializeField] Vector2 _nSize = Vector2.one;
    private void UpdatePoss()
    {
        rtSize = rt.sizeDelta;
        rtPos = rt.AnchoredPosLeftTop();
        rtPos.y = -rtPos.y;
        rtPos.y = UI.scaler.referenceResolution.y - rtPos.y;

        Vector2 posRefN, scl;

        posRefN = rtPos / UI.scaler.referenceResolution;
        scl = rtSize / nSize / UI.scaler.referenceResolution;
        mtx_ViewToRect = Matrix4x4.TRS(posRefN, Quaternion.identity, scl);

        posRefN = rtPos;
        scl = rtSize / nSize;
        mtx_RectToRef = Matrix4x4.TRS(posRefN, Quaternion.identity, scl);
        mtx_RefToRect = Matrix4x4.Scale(Vector2.one / scl) * Matrix4x4.Translate(-posRefN);
    }
    bool picking;
    private float leftTimer;
    private float rightTimer;
    private float upTimer;
    private float downTimer;
    public float continuousKeyTime = 0.2f; // 上下左右键连发延迟
    public float continuousKeyInterval = 0.01f; // 间隔（其实0.01通常约等于每帧触发）
    void GetInput()
    {
        if (!gameObject.activeSelf || !enabled) return;

        if (_curveIdx_pre != curveIdx)
        {
            GetCurve();
            keySel = null;
            _curveIdx_pre = curveIdx;
            var curveIdx_Names = new string[]
            //{ "Pos X", "Pos Y" , "Pos Z", "Rot X", "Rot Y", "Rot Z", };
            { "Pos X", "Pos Y" , "Pos Z", "Euler X", "Euler Y", "Euler Z", };
            curveIdx_text.text = curveIdx_Names[curveIdx];
        }

        this.CheckResize(UpdatePoss);

        if (curve == null) return;

        if (Vector2.Distance(Input.mousePosition, prevPos) > moveError) { selIdx = 0; move = true; }

        mousePosRf = UI.mousePosRef_LB;
        mousePosRectN = mtx_RefToRect.MultiplyPoint(mousePosRf);

        pts = new List<Vector2>();
        var ks = new List<Key2>();
        var idx = new List<int>();
        foreach (var key in curve)
        {
            pts.Add(mtx_RectToRef.MultiplyPoint(key.vector));
            ks.Add(key);
            idx.Add(0);
            if (keySel != key) continue;
            if (key.inMode == KeyMode.Bezier)
            {
                pts.Add(mtx_RectToRef.MultiplyPoint(key.inTangent));
                ks.Add(key);
                idx.Add(1);
            }
            if (key.outMode == KeyMode.Bezier)
            {
                pts.Add(mtx_RectToRef.MultiplyPoint(key.outTangent));
                ks.Add(key);
                idx.Add(2);
            }
        }
        if (Events.Mouse1to3) picking = false;
        if (picking) SetKeySelValue();

        if (Events.Key(KeyCode.LeftArrow)) leftTimer += Time.deltaTime; else leftTimer = 0;
        if (Events.Key(KeyCode.RightArrow)) rightTimer += Time.deltaTime; else rightTimer = 0;
        if (Events.Key(KeyCode.UpArrow)) upTimer += Time.deltaTime; else upTimer = 0;
        if (Events.Key(KeyCode.DownArrow)) downTimer += Time.deltaTime; else downTimer = 0;

        if (Events.MouseDown0)
        {
            if (MathTool.Between(mousePosRectN, Vector2.zero, nSize)) dragging = true;
        }
        else if (Events.Mouse0)
        {
            if (dragging && MathTool.Between(mousePosRectN, Vector2.zero, nSize))
            {
                UITimeLine.I.frameIdx_F = mousePosRectN.x;
            }
        }
        //if (Events.MouseDown0 || Events.Mouse0)
        //{
        //    if (MathTool.Between(mousePosRectN, Vector2.zero, nSize))
        //    {
        //        UITimeLine.I.frameIdx_F = mousePosRectN.x;
        //        dragging = true;
        //    }
        //}
        else if (Events.MouseDown1)
        {
            selKeys = new List<Key2>();
            subIdxs = new List<int>();
            oss = new List<Vector2>();

            otherKeys = new List<Key2>();
            otherOss = new List<Vector2>();
            bool click = false; ;
            for (int i = 0; i < pts.Count; i++)
            {
                var rect = new Rt(pts[i], Vector2.one * sizeClick, Vector2.one * 0.5f);
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

                var i = subIdxs[id];
                if (i == 0)
                {
                    foreach (var oc in UIClip.I.clip.curves)
                    {
                        foreach (var curve in oc.curves)
                        {
                            foreach (var key in curve)
                            {
                                if (keySel.time.Approx(key.time, 4))
                                {
                                    otherKeys.Add(key);
                                    otherOss.Add(mousePosRectN - key.vector);
                                }
                            }
                        }
                    }
                }
            }
        }
        else if (Events.Mouse1)
        {
            if (dragging) SetKeySelValue();
        }
        else if (Events.KeyDown(KeyCode.G))
        {
            picking = keySel != null && !picking;
        }
        else
        {
            dragging = false;
            if (Events.KeyDown(KeyCode.A))
            {
                keySel = null;
            }
            else if (Events.KeyDown(KeyCode.R))
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
            else if (upTimer > continuousKeyTime * 1.5f || Events.KeyDown(KeyCode.UpArrow))
            {
                upTimer -= continuousKeyInterval * 1.5f;
                for (int i = curve.Count - 1; i >= 0; i--)
                {
                    if (curve[i].time < UITimeLine.I.frameIdx)
                    {
                        UITimeLine.I.frameIdx_F = curve[i].time;
                        break;
                    }
                }
            }
            else if (downTimer > continuousKeyTime * 1.5f || Events.KeyDown(KeyCode.DownArrow))
            {
                downTimer -= continuousKeyInterval * 1.5f;
                for (int i = 0; i < curve.Count; i++)
                {
                    if (curve[i].time > UITimeLine.I.frameIdx)
                    {
                        UITimeLine.I.frameIdx_F = curve[i].time;
                        break;
                    }
                }
            }
            else if (leftTimer > continuousKeyTime * 1.5f || Events.KeyDown(KeyCode.LeftArrow))
            {
                leftTimer -= continuousKeyInterval;
                UITimeLine.I.frameIdx--;
            }
            else if (rightTimer > continuousKeyTime * 1.5f || Events.KeyDown(KeyCode.RightArrow))
            {
                rightTimer -= continuousKeyInterval;
                UITimeLine.I.frameIdx++;
            }
            else if (Events.KeyDown(KeyCode.I))
            {
                //var k = new Key2();
                //k.vector = mousePosRectN;
                //curve.InsertKey(k);
                UITimeLine.I.InsertKey();
            }
            else if (Events.KeyDown(KeyCode.X))
            {
                if (keySel != null)
                {
                    UITimeLine.I.RemoveKeyAt(keySel.time);
                }
                else
                {
                    UITimeLine.I.RemoveKey();
                    //    var i = subIdxs[id];
                    //    if (i == 0)
                    //    {
                    //        curve.Remove(keySel);
                    //        keySel = null;
                    //        curve.Sort();
                    //    }
                    //    else if (i == 1)
                    //    {
                    //        keySel.inMode = KeyMode.None;
                    //        keySel = null;
                    //        //subIdxs.RemoveAt(id);
                    //    }
                    //    else if (i == 2)
                    //    {
                    //        keySel.outMode = KeyMode.None;
                    //        keySel = null;
                    //        //subIdxs.RemoveAt(id);
                    //    }
                }
            }
        }
        if (dragging) Events.Use();
        prevPos = Input.mousePosition;
        if (mirror)
        {
            curveMirror = curve.Clone();
            curveMirror.Mirror(mirrorError, curve);
        }
    }
}
