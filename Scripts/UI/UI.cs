using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa.UI
{
    public static class UITool
    {
        public static Rt GetRt(this RectTransform rt)
        {
            // anchor 和 pivot 都要上下翻转，转成左上角坐标
            var pos = rt.anchorMin.SubY_L(1) * UI.scaler.referenceResolution;
            pos += rt.anchoredPosition.ReverseY();
            pos += -rt.pivot.SubY_L(1) * rt.rect.size;
            return new Rt(pos, rt.rect.size);
        }
        public static Vector2 ToLT(this Vector2 pos) // input screen pos
        {
            pos.y = UI.scaler.referenceResolution.y - pos.y;
            return pos;
        }
        public static Vector2 ToRefLT(this Vector2 pos) // input screen pos
        {
            pos *= UI.facterToReference;
            pos.y = UI.scaler.referenceResolution.y - pos.y;
            return pos;
        }
    }
    public class UI : Singleton<UI>
    {
        public static float Epsilon = 0.000001f;
        public InputCallBack Find(string name)
        {
            foreach (var cb in inputCallBacks)
            {
                if (name == cb.name) return cb;
            }
            return null;
        }

        public InputCallBack AddInputCB(string name, Action updateFunc, int order)
        {
            return inputCallBacks.Add_R(new InputCallBack(name, updateFunc, order));
        }
        public override void Init()
        {
            I.inputCallBacks = new List<InputCallBack>();
        }
        public List<InputCallBack> inputCallBacks;
        public List<InputCallBack> listCalled;
        public Vector2 _mousePositionRef;
        public virtual int SortList(InputCallBack a, InputCallBack b)
        {
            if (a.order > b.order) { return 1; } ///顺序从低到高
            else if (a.order < b.order) { return -1; }
            return a.name.CompareTo(b.name);
        }
        private void ShowInInspector()
        {
            _mousePositionRef = mousePosRef;
        }
        public void Update()
        {
            ShowInInspector();

            Events.used = false;
            inputCallBacks.Sort(SortList);
            listCalled = new List<InputCallBack>();
            foreach (var call in inputCallBacks)
            {
                if (call.getInput != null)
                {
                    if ((call.mono == null || call.mono.enabled) &&
                        (call.gameObject == null || call.gameObject.activeInHierarchy))
                        call.getInput();
                }
                listCalled.Add(call);
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
            get
            {
                return Screen.width / scaler.referenceResolution.x;
            }
        }
        public static float facterToReference
        {
            get
            {
                return scaler.referenceResolution.x / Screen.width;
            }
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

        internal static bool MouseOver(params RectTransform[] rts)
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
            rect.position = AbsRefPos2(rt);
            //rect.position = MathTool.ReverseY(rt.anchoredPosition);
            //if (rt.name != "Area")
            //{
            //    //rect.position += Vector2.Scale((rt.parent as RectTransform).anchoredPosition, Vector2.one.SetY(-1));
            //    //rect.position += new Vector2(-rt.pivot.x * rt.rect.width, -(1 - rt.pivot.y) * rt.rect.height);
            //    rect.position = AbsRefPos(rt);
            //}
            return rect;
        }
        public static Vector2 AbsRefPos2(RectTransform rt)
        {
            var rtParent = rt.parent as RectTransform;
            Vector2 posParent = Vector2.zero;
            if (rtParent != null)
            {
                posParent = AbsRefPos2(rtParent);
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