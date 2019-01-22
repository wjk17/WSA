using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa.UI_;
namespace Esa
{
    /// <summary>
    /// Unity自带的RectTransform组件，在Inspector按一定顺序切换锚点会导致
    /// AnchoredPosition有略微（1像素左右）偏移，但场景里的实际UI对象不会移动。
    /// </summary>
    [Serializable]
    public class RectTrans
    {
        public Vector2 anchoredPos
        {
            get { return pos + (sizeAbs * pivot - parentPivotV) - anchorMinV; }
            set { pos = value; }
        }
        public Vector2 pos;
        public Vector2 posAbs { get { return parentCorLB + anchoredPos; } }
        public Vector2 sizeAbs;
        public Vector2 parentCorLB { get { return parent == null ? Vector2.zero : parent.cornerLB; } }
        public Vector2 parentPos { get { return parent == null ? Vector2.zero : parent.anchoredPos; } }
        public Vector2 parentPivotV { get { return parentSizeAnchored * pivot; } }
        public Vector2 pivotV { get { return parentSizeAbs * pivot; } }
        public Vector2 pivot;
        public Vector2 anchorMinV { get { return parentSizeAbs * anchorMin; } }
        public Vector2 anchorMin; // 0~1
        public Vector2 anchorMaxV { get { return parentSizeAbs * anchorMax; } }
        public Vector2 anchorMax;

        public Vector2 _offsetMin;
        public Vector2 _offsetMax;
        public Vector2 _sizeDelta;

        public RectTrans parent;
        internal Vector2[] fourCorner { get { return new Vector2[] { cornerLT, cornerLB, cornerRB, cornerRT }; } }
        internal Vector2[] fourCornerGL { get { return new Vector2[] { cornerLB, cornerLT, cornerRT, cornerRB }; } }
        internal Vector2 center { get { return (cornerRT + cornerLB) * 0.5f; } }
        internal Vector2 cornerLT { get { return new Vector2(cornerLB.x, cornerRT.y); } }
        internal Vector2 cornerRB { get { return new Vector2(cornerRT.x, cornerLB.y); } }
        internal Vector2 cornerRT { get { return parentOs + anchorMaxV + offsetMax; } }
        internal Vector2 cornerLB { get { return parentOs + anchorMinV + offsetMin; } }
        internal Vector2 _cornerLB { get { return anchorMinV + offsetMin; } }
        internal Vector2 parentOs
        {
            get
            {
                var p = parent;
                var parentLBAbs = Vector2.zero;
                while (p != null)
                {
                    parentLBAbs += p._cornerLB;
                    p = p.parent;
                }
                return parentLBAbs;
            }
        }

        public Vector2 parentSizeAnchored { get { return parentSizeAbs * anchorMax - parentSizeAbs * anchorMin; } }
        public Vector2 parentSizeAbs { get { return parent == null ? UI_.UI.scalerRefRes : parent.sizeAbs; } }
        public RectTrans(Component c) : this(c.transform as RectTransform) { }
        public RectTrans(RectTransform rt)
        {
            if (rt.parent.GetComponent<Canvas>() == null)
                parent = new RectTrans(rt.parent);
            anchoredPos = rt.GetLBPos();
            pivot = rt.pivot;
            anchorMin = rt.anchorMin;
            anchorMax = rt.anchorMax;
            sizeAbs = rt.rect.size;
            _offsetMin = offsetMin;
            _offsetMax = offsetMax;
            _sizeDelta = sizeDelta;
        }
        public Vector2 sizeDelta // 矩形相对于父矩形的尺寸差
        {
            get
            {
                return sizeAbs - parentSizeAnchored;
            }
            set
            {
                // todo
            }
        }
        public Vector2 offsetMin // 左下角
        {
            get
            {
                return anchoredPos - sizeDelta * pivot;
            }
            set
            {
                Vector2 a = value - (anchoredPos - sizeDelta * pivot);
                sizeDelta -= a;
                anchoredPos += a * (Vector2.one - pivot);
            }
        }

        public Vector2 offsetMax // 右上角
        {
            get
            {
                return anchoredPos + sizeDelta * (Vector2.one - pivot);
            }
            set
            {
                Vector2 a = value - (anchoredPos + sizeDelta * (Vector2.one - pivot));
                sizeDelta += a;
                anchoredPos += a * pivot;
            }
        }
    }
}