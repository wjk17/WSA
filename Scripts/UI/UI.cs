using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa.UI
{
    public partial class UI : Singleton<UI>
    {
        public List<InputCall> inputs;
        public List<InputCall> called;
        public override void Init()
        {
            I.inputs = new List<InputCall>();
        }
        public virtual int SortList(InputCall a, InputCall b)
        {
            if (a.order > b.order) { return 1; } ///顺序从低到高
            else if (a.order < b.order) { return -1; }
            return a.name.CompareTo(b.name);
        }
        public void Update()
        {
            Events.used = false;
            inputs.Sort(SortList);
            called = new List<InputCall>();
            foreach (var call in inputs)
            {
                if (call.getInput != null)
                {
                    if ((call.mono == null || call.mono.enabled) &&
                        (call.gameObject == null || call.gameObject.activeInHierarchy))
                        call.getInput();
                }
                called.Add(call);
                if (call.RT != null) // 如果指定了RT，over时截断其他后续UI事件（used=true）
                {
                    call.rt = call.RT.GetRt();
                    call.mouseOver = call.rt.Contains(mousePosRef);
                    if (call.RT.gameObject.activeInHierarchy && call.mouseOver) return;
                }
                if (Events.used) return;
            }
        }

        private static Canvas _canvas;
        public static Canvas canvas
        {
            get
            {
                if (_canvas == null) _canvas = FindObjectOfType<Canvas>();
                return _canvas;
            }
            set { _canvas = value; }
        }
        private static CanvasScaler _scaler;

        public static CanvasScaler scaler
        {
            get
            {
                if (_scaler == null) _scaler = FindObjectOfType<CanvasScaler>();
                return _scaler;
            }
            set { _scaler = value; }
        }
        public static float facterToRealPixel
        {
            get { return Screen.width / scaler.referenceResolution.x; }
        }
        public static float facterToReference
        {
            get { return scaler.referenceResolution.x / Screen.width; }
        }
        internal static Vector2 mousePosRef // LT
        {
            get
            {
                var ip = Input.mousePosition;
                ip *= facterToReference;
                ip.y = scaler.referenceResolution.y - ip.y;
                return ip;
            }
        }
        internal static Vector2 mousePosRef_LB // LB
        {
            get
            {
                return Input.mousePosition * facterToReference;
            }
        }
        public static bool MouseOver(params Transform[] rts)
        {
            return MouseOver(rts.ToRTs());
        }
        public static bool MouseOver(params RectTransform[] rts)
        {
            var ip = Input.mousePosition;
            ip *= facterToReference;
            ip.y = scaler.referenceResolution.y - ip.y;
            foreach (var rt in rts)
            {
                var rect = GetAbsRect(rt);
                if (rect.Contains(ip)) return true;
            }
            return false;
        }
        public static Rect GetAbsRect(RectTransform rt)
        {
            var rect = rt.rect;
            rect.position = AbsRefPos(rt);
            return rect;
        }
        public static Vector2 AbsRefPos(RectTransform rt)
        {
            var rtParent = rt.parent as RectTransform;
            Vector2 posParent = Vector2.zero;
            if (rtParent != null)
            {
                posParent = AbsRefPos(rtParent);
                //posParent = rtParent.anchoredPosition.ReverseY();
            }
            Vector2 pos = posParent;
            Vector2 anchorPos;
            var amin = rt.anchorMin;
            amin.y = 1 - amin.y;
            var amax = rt.anchorMax;
            amax.y = 1 - amax.y;
            var omin = rt.offsetMin;
            var omax = rt.offsetMax;

            if (amin == amax) // 九宫格锚点模式
            {
                anchorPos = Vector2.Scale(amin, rtParent.rect.size);
                pos += anchorPos + new Vector2(omin.x, -omax.y);
            }
            else if (amin == new Vector2(0, 0) && amax == new Vector2(1, 0))
            {
                pos.y += rtParent.rect.height;
                pos += rt.anchoredPosition.ReverseY();
            }
            else if (amin == new Vector2(0, 0) && amax == new Vector2(0, 1))
            {
                anchorPos = rt.anchoredPosition.ReverseY();
                pos += anchorPos;
            }
            else
            {
                pos += new Vector2(omin.x, -omax.y);// rt.anchoredPosition;
            }
            //p.y = scaler.referenceResolution.y - p.y;
            return pos;
        }
    }
}