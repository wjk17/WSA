using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
namespace Esa.UI_
{
    public class UIMap : Singleton<UIMap>
    {
        public Texture2D texMap;
        public Texture2D texPlayer;
        public float texPlayerSize = 48;
        public Vector2Int viewMapOs;
        Vector2Int prevViewOs;
        public Vector2 viewMapSize;
        Vector2 prevMousePos;
        public Texture2D tex;
        public Vector2[] uv;
        public int frameWidth;
        public float viewScale = 1f;
        RectTrans rt;
        Vector2Int rtSize;
        void Start()
        {
            this.AddInput(Input, 0);
            rt = new RectTrans(this);
            rtSize = rt.sizeAbs.RoundToInt();
            buffer = new Color[rtSize.eleCount()];
            viewMapSize = texMap.Size();
            this.DestroyImages();
        }
        public GraphicsFormat format;
        public TextureCreationFlags flags;
        Color[] buffer;
        [Button]
        void GenTex2D()
        {
            tex = new Texture2D(rtSize.x, rtSize.y, format, flags);
            var texMapSize = texMap.Size();

            viewScale = Mathf.Max(0.4f, viewScale);
            var sampleSize = rtSize.Mul(1f / viewScale);
            viewMapOs = VectorTool.Clamp(viewMapOs, Vector2Int.zero, texMapSize - sampleSize);

            var pixels = texMap.GetPixels(viewMapOs, sampleSize);

            // resample
            for (int y = 0; y < rtSize.y; y++)
            {
                for (int x = 0; x < rtSize.x; x++)
                {
                    var ny = y / (float)(rtSize.y - 1);
                    var nx = x / (float)(rtSize.x - 1);
                    var i = Mathf.RoundToInt(ny * (sampleSize.y - 1)) * sampleSize.x
                        + Mathf.RoundToInt(nx * (sampleSize.x - 1));
                    buffer[y * rtSize.x + x] = pixels[i];
                }
            }

            tex.SetPixels(Vector2Int.zero, rtSize, buffer);

            tex.MaskCorner(UI.I.texWindow[0], UI.I.corSizeWindow);

            tex.Apply(false);
        }
        void Input()
        {
            this.BeginOrtho();
            var vs = UITool.GetVS(rt.cornerLB, rtSize, Vector2.zero);
            GenTex2D();
            GLUI.BeginOrder(0);
            GLUI.DrawTex(tex, vs);

            // frame
            GLUI.BeginOrder(1);
            this.DrawBG(frameWidth, false);
            vs = UITool.GetVS(rt.center, Vector2.one * texPlayerSize, Vectors.half2d);
            GLUI.DrawTex(texPlayer, vs);

            if (Events.MouseDown0)
            {
                prevMousePos = UI.mousePosRef;
                prevViewOs = viewMapOs;
            }
            else if (Events.Mouse0)
            {
                var os = (UI.mousePosRef - prevMousePos).RoundToInt();
                os = (os * Vector2.one / viewScale).RoundToInt();
                viewMapOs = prevViewOs - os;
            }
        }
    }
}