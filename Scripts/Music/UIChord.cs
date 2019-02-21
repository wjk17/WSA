using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using UI_;
    public class UIChord : Singleton<UIChord>
    {
        public int idx = -1;
        public RectTransform chordNum;
        public RectTransform rtSS; //ScaleShift
        public Color colorSSOn = Color.green;
        public Color colorSSOff = Color.grey;
        public int fontSizeNum = 42;
        public string[] names;
        public Vector2 sizeOs;
        public Color colorSel = Color.blue;
        public Color colorNormal = Color.white;
        public bool scaleShift;
        void Start()
        {
            this.AddInput(Input, 2, false);
        }
        void Input()
        {
            this.BeginOrtho(5);
            GLUI.SetFontColor(Color.black);
            //this.Draw(Palette.L8, true);
            //this.Draw(Color.black, false);

            var rtNum = new RectTrans(chordNum);
            GLUI.SetFontColor(Color.black);
            var sNum = "(" + InstKeyboard.I.chordIdx.ToString() + ")";
            GLUI.DrawString(sNum, rtNum.center, fontSizeNum, Vectors.half2d);

            var rtSS = new Rect(this.rtSS);
            rtSS.Draw(scaleShift ? colorSSOn : colorSSOff, true);
            rtSS.Draw(Color.black, false);

            var rt = new RectTrans(this);
            var startpos = rt.center;

            var os = rt.sizeAbs.x / names.Length;
            var ps = startpos.Average(os, names.Length, Vectors.halfRight2d);

            for (int i = 0; i < names.Length; i++)
            {
                var rect = new Rect(ps[i], os * Vector2.right + sizeOs, Vectors.half2d);
                if (idx == i)
                {
                    rect.Draw(colorSel, true);
                    GLUI.SetFontColor(Color.white);
                    GLUI.DrawString(names[i], ps[i]);
                }
                else
                {
                    rect.Draw(colorNormal, true);
                    GLUI.SetFontColor(Color.black);
                    GLUI.DrawString(names[i], ps[i]);
                }
                rect.Draw(Color.black, false);
            }

        }
    }
}