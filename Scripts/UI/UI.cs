using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa.UI
{
    public partial class UI : Singleton<UI>
    {
        [Header("CallBack")]
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
        public bool seeCalledList = true;
        public void Update()
        {
            Events.used = false;
            inputs.Sort(SortList);
            called = new List<InputCall>();
            foreach (var call in inputs)
            {
                if (!call.on) continue;
                if (call.getInput != null)
                {
                    call.enabled = call.mono == null ||
                        (call.mono.enabled && call.gameObject.activeInHierarchy);

                    if (call.enabled)
                    {
                        call.getInput();
                        if (seeCalledList) called.Add(call);
                    }
                }
                // 如果指定了checkOver，mouseOver 时截断其他后续UI事件（used=true）
                if (call.checkOver && call.RT != null)
                {
                    call.rt = new Rect(call.RT);
                    call.mouseOver = call.rt.Contains(mousePosRef_LB);
                    if (call.mouseOver) return;
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
        public static bool MouseOver(params MonoBehaviour[] monos)
        {
            return MouseOver(monos.ToRTs());
        }
        public static bool MouseOver(params Transform[] rts)
        {
            return MouseOver(rts.ToRTs());
        }
        public static bool MouseOver(params RectTransform[] rts)
        {
            foreach (var rt in rts)
            {
                var rect = GetAbsRect(rt);
                if (rect.Contains(mousePosRef_LB)) return true;
            }
            return false;
        }
        public static Rect GetAbsRect(RectTransform rt)
        {
            return new Rect(rt);
        }
        static Vector2 AbsRefPos_Root(RectTransform rt)
        {
            var pos = rt.anchoredPosition;
            pos += rt.anchorMin * scaler.referenceResolution;
            pos += rt.rect.size * Vectors.half2d;
            pos -= rt.pivot * rt.rect.size;
            return pos;
        }
        public static Vector2 AbsRefPos(Transform t)
        {
            return AbsRefPos(t as RectTransform);
        }
        public static Vector2 AbsRefPos(RectTransform rt)
        {
            var pos = Vector2.zero;
            var rtParent = rt.parent as RectTransform;
            if (rtParent == null || rtParent.GetComponent<Canvas>() != null)
                return AbsRefPos_Root(rt);
            else
                pos += AbsRefPos(rtParent);

            ///

            var anchoredPos = rt.anchoredPosition;
            var amin = Vector2.Scale(rt.anchorMin - Vectors.half2d, rtParent.rect.size);
            var amax = Vector2.Scale(rt.anchorMax - Vectors.half2d, rtParent.rect.size);

            pos += amin + anchoredPos + rt.rect.size * Vectors.half2d;
            pos -= rt.pivot * rt.rect.size;

            return pos;
        }
    }
}