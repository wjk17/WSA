﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Esa.UI
{
    public partial class UICurve
    {
        public static Curve2 Curve
        {
            set
            {
                I.keySel = null;
                I.curve = value;
                I.gameObject.SetActive(value != null);
            }
            get { return I.curve; }
        }
        Curve2 curve;
        public List<Vector2> pts;

        public float sizeClick = 30; //小正方形可点击区域的边长    

        public float sizeDrawTangent = 25;
        public float sizeDrawVector = 20;

        public int selectInt;

        Matrix4x4 m_Curve_Ref;
        private Matrix4x4 m_Ref_Curve;
        public Vector2 mousePosRef;

        [SerializeField] bool dragging;
        public Key2 keySel;
        List<Vector2> oss;
        [SerializeField] Vector2 os;

        public Vector2 mousePosCurve;
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
        public bool showTangentsUnSel;

        Vector2 _rtPos;
        Vector2 rtPos { get { return new Rect(rt).LB(); } }
        //rt.rect.size; 
        Vector2 rtSize { get { return rt.rect.size; } } // 曲线视图区域大小
        public RectTransform rt { get { return (transform as RectTransform); } }
    }
}