using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Esa.UI_
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
        public Color drawBorderClr = Color.black;
        public bool drawTips;

        public Vector2Int gridCount = new Vector2Int(8, 3);
        public Vector2 gridOs = new Vector2(5, 10);
        public Vector2 gridOsFactor = Vector2.one;
        public Vector2 gridSize = new Vector2(80, 80);

        public Color colorNormal = Color.grey;
        public Color colorOver = Color.white;
        public Color colorDown = Color.grey;
        public Action<int> onClick;
        public Action<int> onOver;

        public Vector2 fontBorder = new Vector2(30, 0);
        public Vector2 osFactor = new Vector2(-0.5f, 0.5f);
        public Vector2 offset = new Vector2(-5, 5);

        public Vector2 nameOffset;

        public bool initOnStart = true;
        public int drawOrder = 0;

        InputCall ic;
        public Vector2 pivot = Vectors.half2d;
        public Color fontColor = Color.black;
        public int buttonStyle = 0;
        void Start()
        {
            if (initOnStart) Initialize();
        }
        public void Initialize()
        {
            if (SYS.debugUI) print("UIGrid Initialize");

            this.DestroyImages();
            ic = this.AddInput(Input, drawOrder, false);
        }
        public void Input()
        {
            ic.order = drawOrder;
            this.BeginOrtho();
            var rt = new RectTrans(this);
            var startPos = rt.center;
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
                GLUI.fontColor = fontColor;
                GLUI.BeginOrder(0);

                if (!clickable[i]) DrawButton(rt, 2);
                else
                {
                    if (rt.Contains(UI.mousePosRef) && clickable[i])
                    {
                        OnOver(i);
                        if (Events.Mouse1to3)
                        {
                            DrawButton(rt, 2);
                            if (Events.MouseDown1to3 && !clicked)
                            {
                                OnClick(i);
                                clicked = true;
                            }
                        }
                        else
                        {
                            DrawButton(rt, 1);
                            if (drawTips)
                            {
                                // tips
                                var str = names[i];
                                var size = IMUI.CalSize(str);
                                size += fontBorder;

                                var os = offset + osFactor * size;
                                GLUI.DrawString(str, UI.mousePosRef + os, Vectors.half2d);
                                GLUI.BeginOrder(3);
                                var bg = new Rect(UI.mousePosRef + os, size, Vectors.half2d);
                                bg.Draw(Color.white, true);
                            }
                        }
                    }
                    else DrawButton(rt, 0);
                    if (drawName)
                    {
                        GLUI.DrawString(names[i], (rt.pos + nameOffset), Vectors.half2d);
                    }
                    GLUI.BeginOrder(1);
                    if (textures.NotEmpty() && textures[i] != null)
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
        public bool useSkin = true;
        Color GetColor(int status)
        {
            switch (status)
            {
                case 0: return colorNormal;
                case 1: return colorOver;
                case 2: return colorDown;
                default: throw new Exception();
            }
        }
        void DrawButton(Rect rt, int status)
        {
            if (useSkin)
                rt.DrawButton(buttonStyle, GetColor(status), status);
            else
                rt.Draw(colorNormal, true);
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
