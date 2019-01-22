using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    public static partial class UITool
    {
        public static Vector2 DivRef(this Vector2 v)
        {
            return v / UI.scalerRefRes;
        }
        public static Vector2 MulRef(this Vector2 v)
        {
            return v * UI.scalerRefRes;
        }
        public static Vector3 MulRef(this Vector3 v)
        {
            return v * UI.scalerRefRes;
        }
        public static Vector2 AbsRefPos(this MonoBehaviour mono)
        {
            return UI.AbsRefPos(mono.transform as RectTransform);
        }
        public static Vector2 AbsRefPos(this Transform t)
        {
            return UI.AbsRefPos(t as RectTransform);
        }
        public static bool MouseOver(this MonoBehaviour mono)
        {
            return UI.MouseOver(mono.transform as RectTransform);
        }
        public static void StartIM(this MonoBehaviour mono)
        {
            UI.owner = mono.gameObject;
            UI.ClearIM();
        }
        public static void StartGLWorld(this MonoBehaviour mono, int order = 0)
        {
            UI.owner = mono.gameObject;
            UI.ClearGL();
            UI.gl.order = order;
            GLUI.BeginOrder(0);
        }
        public static void StartGL(this MonoBehaviour mono)
        {
            UI.owner = mono.gameObject;
            UI.ClearGL();
            GLUI.BeginOrder(0);
            GLUI.LoadOrtho();
        }
        public static void BeginOrtho(this object obj, int order = 0)
        {
            UI.owner = obj;
            UI.ClearCmd();
            UI.gl.order = order;
            GLUI.BeginOrder(0);
            GLUI.LoadOrtho();
        }
        public static void BeginOrtho(this MonoBehaviour mono, int glOrder = 0)
        {
            UI.owner = mono.gameObject;
            UI.ClearCmd();
            if (glOrder != 0) UI.gl.order = glOrder;
            GLUI.BeginOrder(0);
            GLUI.LoadOrtho();
        }
        public static List<Vector2> ListReverseY(this IList<Vector2> vs) // input screen pos
        {
            for (int i = 0; i < vs.Count; i++)
            {
                vs[i] = vs[i].ReverseY();
            }
            return new List<Vector2>(vs);
        }
        public static Vector2 ToLB(this Vector2 pos)
        {
            pos.y = UI.scaler.referenceResolution.y - pos.y;
            return pos;
        }
        public static Vector2 ToLT(this Vector2 pos)
        {
            pos.y = UI.scaler.referenceResolution.y - pos.y;
            return pos;
        }
        public static Vector2 ToLTScreen(this Vector2 pos)
        {
            pos.y = Screen.height - pos.y;
            return pos;
        }
        public static Vector3 XYToNDC(this Vector3 p) // input screen pos
        {
            return (p.XY() / UI.scaler.referenceResolution).SetZ(p.z);
        }
        public static Vector2 ToNDC(this Vector2 p) // input screen pos
        {
            return p /= UI.scaler.referenceResolution;
        }
        public static Vector2 ToNDCLT(this Vector2 p) // input screen pos
        {
            p /= UI.scaler.referenceResolution;
            p.y = 1 - p.y;
            return p;
        }
        public static Vector2 ToRefLT(this Vector2 pos) // input screen pos
        {
            pos *= UI.facterToReference;
            pos.y = UI.scaler.referenceResolution.y - pos.y;
            return pos;
        }
    }
}