﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI
{
    public static partial class UITool
    {
        public static Vector2 MulRef(this Vector2 v)
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
            UI.owner = mono.transform as RectTransform;
            UI.ClearIM();
        }
        public static void StartGL(this MonoBehaviour mono)
        {
            UI.owner = mono.transform as RectTransform;
            UI.ClearGL();
            GLUI.BeginOrtho();
            GLUI.BeginOrder(0);
        }
        public static void FrameStart(this MonoBehaviour mono)
        {
            UI.owner = mono.transform as RectTransform;
            UI.ClearCmd();
            GLUI.BeginOrtho();
            GLUI.BeginOrder(0);
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