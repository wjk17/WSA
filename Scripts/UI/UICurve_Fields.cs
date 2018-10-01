﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UICurve
{
    public Curve2 Curve
    {
        set
        {
            keySel = null;
            curve = value;
            gameObject.SetActive(value != null);
        }
        get { return curve; }
    }
    public Curve2 curve;
    public List<Vector2> pts;

    public float sizeClick = 30; // 所有控制点可点击区域（正方形）的边长    
    public float sizeDrawTangent = 25; // 锚点菱形显示大小
    public float sizeDrawVector = 20; // 主点方形显示大小

    Vector2 rtSize; // 曲线视图区域大小
    Vector2 rtPos;

    public int selectInt;

    Matrix4x4 mtx_ViewToRect;
    Matrix4x4 mtx_RectToRef;
    private Matrix4x4 mtx_RefToRect;
    public Vector2 mousePosRf;

    public bool dragging;
    Key2 keySel;
    void SetKeySelValue()
    {
        if (keySel == null) return;
        var i = subIdxs[id];
        if (i == 0)
        {
            keySel.SetVector(-os + mousePosRectN);
            for (int k = 0; k < otherKeys.Count; k++)
            {
                //otherKeys[k].SetVector(-otherOss[k] + mousePosRectN);//sync vector
                otherKeys[k].SetTime(-otherOss[k].x + mousePosRectN.x);//sync time
            }
            curve.Sort();
        }
        else if (i == 1)
            keySel.inTangent = -os + mousePosRectN;
        else if (i == 2)
            keySel.outTangent = -os + mousePosRectN;
    }
    List<Key2> otherKeys;
    List<Vector2> otherOss;
    List<Vector2> oss;
    Vector2 os;

    public Vector2 mousePosRectN;
    public Vector2 prevPos;
    public List<Key2> selKeys;
    public List<int> subIdxs;
    public int selIdx;
    public bool move;
    public float moveError = 2;
    public int id;

    public Curve2 curveMirror;
    public bool mirror;
    public float mirrorError = 0.01f;

    RectTransform rt { get { return (transform as RectTransform); } }
}