using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI
{
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
        Curve2 curve;
        public List<Vector2> pts;

        public float sizeClick = 30; //小正方形可点击区域的边长    

        public float sizeDrawTangent = 25;
        public float sizeDrawVector = 20;

        public int selectInt;

        Matrix4x4 m_Curve_V;
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

        public Vector2 _rtPos;
        Vector2 rtPos
        {
            get
            {
                _rtPos = rt.anchoredPosition;
                _rtPos.y = -_rtPos.y;
                _rtPos.y = UI.scaler.referenceResolution.y - _rtPos.y;
                return _rtPos;
            }
        }
        //rt.rect.size; 
        Vector2 rtSize { get { return rt.sizeDelta; } } // 曲线视图区域大小
        RectTransform rt { get { return (transform as RectTransform); } }
    }
}