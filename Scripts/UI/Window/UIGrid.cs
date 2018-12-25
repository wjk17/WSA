using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Esa.UI
{
    public class UIGrid : MonoBehaviour
    {
        List<Rect> rects;
        public List<bool> clickable;
        public List<string> names;
        public List<Texture2D> textures;

        public bool drawName;
        public bool drawBorder;
        public Color drawBorderClr;
        public bool drawTips;

        public Vector2Int gridCount = new Vector2Int(8, 3);
        public Vector2 gridOs = new Vector2(5, 10);
        public Vector2 gridOsFactor = Vector2.one;
        public Vector2 gridSize = new Vector2(80, 80);

        public Color colorNormal;
        public Color colorOver;
        public Color colorDown;
        public Action<int> onClick;
        public Action<int> onOver;

        public Vector2 fontBorder = new Vector2(30, 0);
        public Vector2 osFactor = new Vector2(-0.5f, 0.5f);
        public Vector2 offset = new Vector2(-5, 5);

        public bool initOnStart = true;
        private void Reset()
        {
            colorNormal = Color.grey;
            colorOver = Color.white;
            colorDown = Color.grey;
        }
        void Start()
        {
            if (initOnStart) Initialize();
        }
        public void Initialize()
        {
            if (SYS.debugUI) print("UIGrid Initialize");
            this.AddInput(Input, 0, false);
            {
                foreach (var img in transform.GetComChildren<Image>())
                {
                    Destroy(img);
                }
            }
        }
        public void Input()
        {
            this.FrameStart();
            var startPos = this.AbsRefPos();
            rects = new List<Rect>();
            for (int y = 0; y < gridCount.y; y++)
            {
                for (int x = 0; x < gridCount.x; x++)
                {
                    rects.Add(new Rect(startPos + gridOsFactor * new Vector2(x, y) *
                        (gridOs + gridSize), gridSize));
                }
            }
            var i = 0; bool clicked = false;
            foreach (var rt in rects)
            {
                GLUI.BeginOrder(0);

                if (!clickable[i]) rt.Draw(colorDown, true);
                else
                {
                    if (rt.Contains(UI.mousePosRef) && clickable[i])
                    {
                        rt.Draw(colorOver, true);
                        OnOver(i);
                        if (drawTips)
                        {
                            // tips
                            var str = names[i];
                            var size = IMUI.CalSize(str);
                            size += fontBorder;

                            var os = offset + osFactor * size;
                            IMUI.DrawText(str, UI.mousePos + os * UI.facterToRealPixel, Vectors.half2d);
                            GLUI.BeginOrder(3);
                            var bg = new Rect(UI.mousePosRef + os, size, Vectors.half2d);
                            bg.Draw(Color.white, true);
                        }
                        if (Events.MouseDown1to3 && !clicked)
                        {
                            OnClick(i);
                            clicked = true;
                        }
                    }
                    else rt.Draw(colorNormal, true);
                    if (drawName)
                    {
                        IMUI.DrawText(names[i], rt.pos * UI.facterToRealPixel, Vectors.half2d);
                    }
                    GLUI.BeginOrder(1);
                    if (textures[i] != null)
                    {
                        GLUI.DrawTex(textures[i], rt.ToPointsCWLT(-1));
                    }

                    GLUI.SetLineMat();
                    GLUI.BeginOrder(0);
                }
                // 待做优化 tex和line分开两个loop
                GLUI.BeginOrder(2);
                if (drawBorder) rt.Draw(drawBorderClr, false);
                i++;
            }
        }
        void OnOver(int idx)
        {
            if (onOver != null) onOver(idx);
        }
        void OnClick(int idx)
        {
            if (SYS.debugUI) print("ItemClick");
            if (onClick != null) onClick(idx);
        }
    }
}
