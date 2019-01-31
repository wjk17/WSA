using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa.UI_
{
    [Serializable]
    public class _Cursor
    {
        public Texture2D icon;
        public Vector2 hotspot;
    }
    [ExecuteInEditMode]
    public partial class UI : Singleton<UI>
    {
        public Font font;
        public int defaultFontSize = 32;
        public static string layerName = "UI";
        public static LayerMask layerMask { get { return LayerMask.GetMask(layerName); } }
        public static int layerNum { get { return (int)Mathf.Log(layerMask.value, 2); } }

        public _Cursor cursorOverNPC;
        public _Cursor cursorDefault;
        public static Transform Root
        {
            get { return canvas.transform; }
        }
        [Header("CallBack")]
        public List<InputCall> inputs;
        public List<InputCall> _inputs;
        public List<InputCall> called;
        public bool seeCalledList;
        public virtual int SortList(GLHandler a, GLHandler b)
        {
            if (a.order > b.order) { return 1; } ///顺序从低到高
            else if (a.order < b.order) { return -1; }

            return ((GameObject)a.owner).name.CompareTo(((GameObject)b.owner).name);
        }
        public virtual int SortList(InputCall a, InputCall b)
        {
            if (a.order > b.order) { return 1; } ///顺序从低到高
            else if (a.order < b.order) { return -1; }
            return a.name.CompareTo(b.name);
        }
        public Material texMaterial;
        [Button]
        public void Initialize()
        {
            if (SYS.debugUI) print("UI Initialize.");

            GLUI.texMaterial = texMaterial;
            GLUI.font = font;

            inputs = new List<InputCall>();
            _inputs = new List<InputCall>();
            glHandlers = new List<GLHandler>();
            imHandlers = new List<IMHandler>();
            var wrapper = camera.GetComOrAdd<CameraEventWrapper>();
            wrapper.onPostRender = CameraPostRender;
        }
        private void Update()
        {
            GLUI.fontSize = defaultFontSize;
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            Events.Update();
            EarlyUpdate();
            UpdateInput();
        }
        public Action earlyUpdate;
        public void EarlyUpdate()
        {
            if (earlyUpdate != null) earlyUpdate();
        }
        public void UpdateInput()
        {
            Events.used = false;
            if (_inputs.Count > 0)
            {
                inputs.AddRange(_inputs);
                _inputs.Clear();
            }
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
                    call.mouseOver = call.rt.Contains(mousePosRef);
                    if (call.mouseOver) return;
                }
                if (Events.used) return;
            }
        }
        public void StartCoro(System.Collections.IEnumerator ie)
        {
            StartCoroutine(ie);
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
        public float screenFactor
        {
            get { return Screen.currentResolution.width / Screen.currentResolution.height; }
        }
        public Vector2 screenSize
        {
            get { return new Vector2(Screen.width, Screen.height); }
        }
        public static float facterToRealPixel
        {
            get { return Screen.width / scaler.referenceResolution.x; }
        }
        public static float facterToReference
        {
            get { return scaler.referenceResolution.x / Screen.width; }
        }
        public static Vector2 scalerRefRes
        {
            get { return scaler.referenceResolution; }
        }
        public static Vector2 mousePos // LT
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying) return I.mousePosEvent;
#endif
                return Input.mousePosition;
            }
        }
        internal static Vector2 mousePosRef_LT // LT
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    return (I.mousePosEvent / I.gameViewSize).MulRef();
#endif
                return (Input.mousePosition * facterToReference).
                    f_sub_y(scaler.referenceResolution.y);
            }
        }
        public static Vector2 mousePosView // LB 本来就是左下坐标
        {
            get
            {
                return Input.mousePosition / I.screenSize;
            }
        }
        public static Vector2 mousePosRef // LB 本来就是左下坐标
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    return (I.mousePosEvent / I.gameViewSize).FlipY().MulRef();
#endif
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
                if (rect.Contains(mousePosRef)) return true;
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
        public static Vector2 AbsRefPos(MonoBehaviour mono)
        {
            return AbsRefPos(mono.transform as RectTransform);
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