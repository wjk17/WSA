using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa.UI_
{
    [Serializable]
    public class ColorList
    {
        public List<Color> colors;
        public List<String> names;
        public ColorList()
        {
            colors = new List<Color>();
            names = new List<String>();
        }
        public ColorList Clone()
        {
            return new ColorList()
            {
                colors = new List<Color>(colors),
                names = new List<string>(names)
            };
        }
        internal void AddRange(ColorList list)
        {
            colors.AddRange(list.colors);
            names.AddRange(list.names);
        }
    }
    public class UIColorPicker : MonoBehaviour
    {
        public RectTransform preview;
        public Color color;
        public RectTransform thumbnail;
        public Vector2Int gridCount = new Vector2Int(8, 3);
        public Vector2 gridOs = new Vector2(5, 10);
        public Vector2 gridOsFactor = Vector2.one;
        public float gridSize = 25f;
        public List<ColorList> colorList;
        void Start()
        {
            foreach (var img in transform.GetComChildren<Image>())
            {
                Destroy(img);
            }
            this.AddInput(Input, 0, false);
        }
        private void Reset()
        {
            LerpPalette();
        }
        [Button]
        void PaletteToComponent()
        {
            var cp = gameObject.AddComponent<ColorPalette>();
            cp.list = new ColorList();
            foreach (var list in colorList)
            {
                cp.list.AddRange(list);
            }
        }
        public string[] matNamePrefixs = new string[] { "浅棕", "暗棕", "深棕" };
        [Button]
        private void LerpPalette()
        {
            float upper = gridCount.x - 1;

            for (int y = 0; y < gridCount.y; y++)
            {
                for (int x = 1; x < gridCount.x - 1; x++)
                {
                    var a = colorList[y].colors[0];
                    var b = colorList[y].colors[gridCount.x - 1];
                    colorList[y].colors[x] = a.Lerp(b, x / upper);
                }
            }

            for (int y = 0; y < gridCount.y; y++)
            {
                for (int x = 0; x < gridCount.x; x++)
                {
                    var a = colorList[y].colors[0];
                    var b = colorList[y].colors[gridCount.x - 1];
                    colorList[y].names[x] = matNamePrefixs[y] + (x + 1).ToString();
                }
            }
        }
        [Button]
        void RGBPalette()
        {
            float upper = gridCount.x - 1;

            for (int i = 0; i < gridCount.x; i++)
                colorList[0].colors[i] = (new Color(i / upper, 0, 0));

            for (int i = 0; i < gridCount.x; i++)
                colorList[1].colors[i] = (new Color(0, i / upper, 0));

            for (int i = 0; i < gridCount.x; i++)
                colorList[2].colors[i] = (new Color(0, 0, i / upper));
        }
        public Color tipsBgColor;
        public Color tipsTxtColor;
        void Input()
        {
            this.BeginOrtho();
            Thumbnail();
            Tips();
        }
        public Vector2 fontBorder = new Vector2(30, 0);
        public Vector2 osFactor = new Vector2(-0.5f, 0.5f);
        public Vector2 offset = new Vector2(-5, 5);
        void Tips()
        {
            this.StartIM();
            for (int i = 0; i < rects.Count; i++)
            {
                var rt = rects[i];
                if (rt.Contains(UI.mousePosRef))
                {
                    //show tips
                    IMUI.fontStyle.normal.textColor = tipsTxtColor;
                    var str = tips[i];
                    var size = IMUI.CalSize(str);
                    size += fontBorder;

                    var os = offset + osFactor * size;
                    GLUI.DrawString(str, UI.mousePosRef + os, Vectors.half2d);
                    GLUI.BeginOrder(2);
                    var bg = new Rect(UI.mousePosRef + os, size, Vectors.half2d);
                    bg.Draw(tipsBgColor, true);

                    if (Events.MouseDown1to3)
                    {
                        Select(i);
                        return;
                    }
                }
            }
        }
        public void Select(int x, int y)
        {
            Select(y * gridCount.x + x);
        }
        public void Select(int i)
        {
            preview.GetComponentInChildren<Text>().text = tips[i];
            color = palette[i];
        }

        List<Rect> rects;
        [NonSerialized] public List<Color> palette;
        [NonSerialized] public List<string> tips;

        public bool drawBorder;
        public Color drawBorderClr;

        public Color drawPvBorderClr;
        public float drawPvBorderWidth;
        void Thumbnail()
        {
            var startPos = UI.AbsRefPos(thumbnail);
            rects = new List<Rect>();
            //palette = new List<Color>();
            //tips = new List<string>();
            for (int y = 0; y < gridCount.y; y++)
            {
                for (int x = 0; x < gridCount.x; x++)
                {
                    rects.Add(new Rect(startPos + gridOsFactor * new Vector2(x, y) *
                        (gridOs + Vector2.one * gridSize), gridSize));
                    //palette.Add(colorList[y].colors[x]);
                    //tips.Add(colorList[y].names[x]);
                }
            }
            var i = 0;
            foreach (var rt in rects)
            {
                GLUI.BeginOrder(0);
                rt.Draw(palette[i++], true);
                GLUI.BeginOrder(1);
                if (drawBorder) rt.Draw(drawBorderClr, false);
            }
            GLUI.BeginOrder(0);
            preview.Draw(color, true);
            GLUI.BeginOrder(1);
            preview.Draw(drawPvBorderClr);
        }
    }
}