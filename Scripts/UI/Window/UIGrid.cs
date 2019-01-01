using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Esa.UI
{
    public class UIGrid : MonoBehaviour
    {
        [NonSerialized]
        public List<Rect> rects;
        public List<bool> clickable;
        public List<bool> visible;
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

        public Vector2 nameOffset;

        public bool initOnStart = true;
        public int _drawOrder = 0;
        public int drawOrder = 0;
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
            this.AddInput(Input, drawOrder, false);
            {
                foreach (var img in transform.GetComChildren<Image>())
                {
                    Destroy(img);
                }
            }
        }
        public Vector2 pivot = Vectors.half2d;
        public Color fontColor = Color.black;

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
                        (gridOs + gridSize), gridSize, pivot));
                }
            }
            var i = 0; bool clicked = false;
            foreach (var rt in rects)
            {
                if (visible.NotEmpty() && !visible[i]) { i++; continue; }
                IMUI.fontColor = fontColor;
                GLUI.BeginOrder(0 + _drawOrder);

                if (!clickable[i]) rt.Draw(colorDown, true);
                else
                {
                    if (rt.Contains(UI.mousePosRef) && clickable[i])
                    {
                        OnOver(i);
                        if (Events.Mouse1to3)
                        {
                            rt.Draw(colorDown, true);
                            if (Events.MouseDown1to3 && !clicked)
                            {
                                OnClick(i);
                                clicked = true;
                            }
                        }
                        else
                        {
                            rt.Draw(colorOver, true);
                            if (drawTips)
                            {
                                // tips
                                var str = names[i];
                                var size = IMUI.CalSize(str);
                                size += fontBorder;

                                var os = offset + osFactor * size;
                                IMUI.DrawText(str, UI.mousePos + os * UI.facterToRealPixel, Vectors.half2d);
                                GLUI.BeginOrder(3 + _drawOrder);
                                var bg = new Rect(UI.mousePosRef + os, size, Vectors.half2d);
                                bg.Draw(Color.white, true);
                            }
                        }
                    }
                    else rt.Draw(colorNormal, true);
                    if (drawName)
                    {
                        IMUI.DrawText(names[i], (rt.pos + nameOffset) * UI.facterToRealPixel, Vectors.half2d);
                    }
                    GLUI.BeginOrder(1 + _drawOrder);
                    if (textures[i] != null)
                    {
                        GLUI.DrawTex(textures[i], rt.ToPointsCWLT(-1));
                    }

                    GLUI.SetLineMat();
                    GLUI.BeginOrder(0 + _drawOrder);
                }
                // 待做优化 tex和line分开两个loop
                GLUI.BeginOrder(2 + _drawOrder);
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
            AudioMgr.I.PlaySound("Click");
            if (onClick != null) onClick(idx);
        }
    }
}
