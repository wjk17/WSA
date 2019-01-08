using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI
{
    public static partial class UITool // BG
    {
        public static Vector2[] GetVS(Vector2 pos, Vector2 size, Vector2 pivot)
        {
            return new Vector2[] {
                pos - pivot * size,
                pos + size.Y() - pivot * size,
                pos + size - pivot * size,
                pos + size.X()- pivot * size};
        }
        public static void DrawBG(this Component c, bool solid = true)
        {
            DrawBG(c, UI.I.corSizeWindow, Color.white, solid);
        }
        public static void DrawBG(this Component c, float cornerSize, bool solid = true)
        {
            DrawBG(c, cornerSize, Color.white, solid);
        }
        public static void DrawBG(this Component c, float cornerSize, Color color, bool solid = true)
        {
            var RT = c.transform as RectTransform;
            if (RT == null) return;
            var rt = new RectTrans(RT);

            var rect = new Rect();
            rect.size = rt.sizeAbs;
            rect.pos = rt.center;

            if (solid)
                DrawWindow(rect, cornerSize, color, 0);
            else DrawFrame(rect, cornerSize, color, 0);
        }
        // status 0 normal, 1 hover, 2 down, 3 focus.
        public static void DrawWindow(this Rect rt, Color color, int status)
        {
            DrawBG(rt, UI.I.texWindow[status], UI.I.corSizeWindow, color);
        }
        public static void DrawWindow(this Rect rt, float cornerSize, Color color, int status)
        {
            DrawBG(rt, UI.I.texWindow[status], cornerSize, color);
        }
        public static void DrawFrame(this Rect rt, float cornerSize, Color color, int status)
        {
            DrawBG(rt, UI.I.texWindow[status], cornerSize, color, false);
        }
        public static void DrawButton(this Rect rt, Color color, int status)
        {
            DrawBG(rt, UI.I.texButton[status], UI.I.corSizeButton, color);
        }
        public static void DrawBG(this Rect rt, Texture2D tex, float cornerSize, Color color, bool drawCenter = true)
        {
            var corSize_Ref = Vector2.one * 10f;
            var corSize = Vector2.one * cornerSize;
            var texSize = tex.Size();
            var corSize_uv = corSize_Ref / texSize;
            var w = rt.size.x - corSize.x * 2f;
            var h = rt.size.y - corSize.y * 2f;

            var wS = new Vector2(w, corSize.y);
            var hS = new Vector2(corSize.x, h);

            var wS_uv = new Vector2(texSize.x - corSize_Ref.x * 2, corSize_Ref.y) / texSize;
            var hS_uv = new Vector2(corSize_Ref.x, texSize.y - corSize_Ref.y * 2) / texSize;

            // LBcorner
            var uv = GetVS(Vector2.zero, corSize_uv, Vector2.zero);
            var v = GetVS(rt.cornerLB, corSize, Vector2.zero);
            GLUI.DrawTex(tex, color, v, uv);

            // LT
            uv = GetVS(Vector2.up, corSize_uv, Vector2.up);
            v = GetVS(rt.cornerLT, corSize, Vector2.up);
            GLUI.DrawTex(tex, color, v, uv);

            // RT
            uv = GetVS(Vector2.one, corSize_uv, Vector2.one);
            v = GetVS(rt.cornerRT, corSize, Vector2.one);
            GLUI.DrawTex(tex, color, v, uv);

            // RB
            uv = GetVS(Vector2.right, corSize_uv, Vector2.right);
            v = GetVS(rt.cornerRB, corSize, Vector2.right);
            GLUI.DrawTex(tex, color, v, uv);

            // T
            uv = GetVS(Vector2.up + corSize_uv.X(), wS_uv, Vector2.up);
            v = GetVS(rt.cornerLT + corSize.X(), wS, Vector2.up);
            GLUI.DrawTex(tex, color, v, uv);

            // B
            uv = GetVS(corSize_uv.X(), wS_uv, Vector2.zero);
            v = GetVS(rt.cornerLB + corSize.X(), wS, Vector2.zero);
            GLUI.DrawTex(tex, color, v, uv);

            // L
            uv = GetVS(corSize_uv.Y(), hS_uv, Vector2.zero);
            v = GetVS(rt.cornerLB + corSize.Y(), hS, Vector2.zero);
            GLUI.DrawTex(tex, color, v, uv);

            // R
            uv = GetVS(Vector2.right + corSize_uv.Y(), hS_uv, Vector2.right);
            v = GetVS(rt.cornerRB + corSize.Y(), hS, Vector2.right);
            GLUI.DrawTex(tex, color, v, uv);

            if (drawCenter)
            {
                // Center
                uv = GetVS(corSize_uv, Vector2.one - corSize_uv * 2f, Vector2.zero);
                v = GetVS(rt.cornerLB + corSize, new Vector2(w, h), Vector2.zero);
                GLUI.DrawTex(tex, color, v, uv);
            }
        }
        public static void MaskCorner(this Texture2D tex, Texture2D texMask, float _corSize)
        {
            var corSize = (Vector2.one * _corSize).ToInt();
            var texMaskSize = texMask.Size();
            var texSize = tex.Size();
            var lb = texMask.GetPixels(Vector2Int.zero, corSize);
            var lt = texMask.GetPixels(texMaskSize.Y() - corSize.Y(), corSize);
            var rt = texMask.GetPixels(texMaskSize - corSize, corSize);
            var rb = texMask.GetPixels(texMaskSize.X() - corSize.X(), corSize);
            // LB
            var pixels = tex.GetPixels(Vector2Int.zero, corSize);
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i].a = lb[i].a;
            }
            tex.SetPixels(Vector2Int.zero, corSize, pixels);
            // LT
            pixels = tex.GetPixels(texSize.Y() - corSize.Y(), corSize);
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i].a = lt[i].a;
            }
            tex.SetPixels(texSize.Y() - corSize.Y(), corSize, pixels);
            // RT
            pixels = tex.GetPixels(texSize - corSize, corSize);
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i].a = rt[i].a;
            }
            tex.SetPixels(texSize - corSize, corSize, pixels);
            // RB
            pixels = tex.GetPixels(texSize.X() - corSize.X(), corSize);
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i].a = rb[i].a;
            }
            tex.SetPixels(texSize.X() - corSize.X(), corSize, pixels);
        }
    }
}