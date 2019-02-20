using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Esa.UI_
{
    public class UIGrid_2_0 : MonoBehaviour
    {
        public List<GridUnitProp> gridUnitProp;

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

        public Vector2 margin = new Vector2(5, 5);

        public Vector2 nameOffset;

        public bool initOnStart = true;
        public int inputOrder = 0;
        public int drawOrder = 0;

        public Vector2 pivot = Vectors.half2d;
        public Color fontColor = Color.black;
        public int buttonStyle = 0;

        public bool useSkin = true;

        void Start()
        {
            if (initOnStart) Initialize();
        }
        public void Initialize()
        {
            if (SYS.debugUI) print("UIGrid Initialize");

            this.DestroyImages();
            this.AddInput(Input, inputOrder, false);
        }
        public Vector2 osAbs;
        public void Input()
        {
            this.BeginOrtho(drawOrder);
            var RT = new RectTrans(this);
            var startPos = margin + RT.centerT;

            var psy = startPos.Average(osAbs.Y(), gridCount.y, Vector2.zero);
            for (int y = 0; y < gridCount.y; y++)
            {
                var os = osAbs != Vector2.zero ? osAbs : gridOs + gridSize;
                var ps = psy[y].Average(os.x, gridCount.x, Vectors.halfRight2d);
                for (int x = 0; x < gridCount.x; x++)
                {
                    var n = new Rect(ps[x], gridSize, pivot);
                    gridUnitProp[y * gridCount.x + x].rect = n;
                }
            }
            var i = 0; bool clicked = false;
            foreach (var gup in gridUnitProp)
            {
                if (!gup.visible) { i++; continue; }
                GLUI.SetFontColor(fontColor);
                GLUI.BeginOrder(0);

                if (!gup.clickable)
                {
                    DrawButton(gup, 2);
                    if (drawName)
                        gup.DrawName(nameOffset);

                    if (gup.Hover()) Events.Use();
                }
                else
                {
                    if (gup.Hover() && !Events.used)
                    {
                        OnOver(i);
                        if (Events.Mouse1to3)
                        {
                            DrawButton(gup, 2);
                            if (Events.MouseDown1to3 && !clicked)
                            {
                                OnClick(i);
                                clicked = true;
                            }
                        }
                        else DrawButton(gup, 1);
                        Events.Use();
                    }
                    else DrawButton(gup, 0);

                    if (drawName)
                        gup.DrawName(nameOffset);

                    GLUI.BeginOrder(1);

                    if (gup.texture != null)
                        gup.DrawTexture();

                    GLUI.SetLineMat();
                    GLUI.BeginOrder(0);
                }
                // 待做优化 tex和line分开两个loop
                GLUI.BeginOrder(2);
                if (drawBorder)
                    gup.rect.Draw(drawBorderClr, false);
                i++;
            }
        }
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
        void DrawButton(GridUnitProp p, int status)
        {
            if (useSkin)
                p.rect.DrawButton(buttonStyle, GetColor(status), status);
            else
                p.rect.Draw(GetColor(status), true);
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
