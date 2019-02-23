using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    public class UICornerLT : Singleton<UICornerLT>
    {
        public Texture2D headTex;
        public RectTransform headRT;
        public RectTransform propRT;
        public RectTransform nameRT;
        void Start()
        {
            this.AddInput(_Input, 10);
            this.DestroyImages();
        }
        void _Input()
        {
            this.BeginOrtho();
            this.DrawBG();
            var rtHead = new Rect(headRT);
            var rtName = new Rect(nameRT);
            var rtProp = new Rect(propRT);
            GLUI.DrawTex(headTex, UITool.GetVS(rtHead.cornerLB, rtHead.size, Vector2.zero));
            var P = CharCtrl.I.P;

            GLUI.DrawString(rtName.center, P.charName, Vectors.half2d);            
            GLUI.DrawString(rtProp.cornerLB, "生命: " + P.hp + "/" + P.hpMax);
            GLUI.DrawString(rtProp.cornerLB + -28.Y(), "魔法: " + P.mp + "/" + P.mpMax);
            GLUI.DrawString(rtProp.cornerLB + -28 * 2.Y(), "经验值: " + P.exp + "/" + P.expMax);
            GLUI.DrawString(rtProp.cornerLB + -28 * 3.Y(), "等级: " + P.lvl);
        }
    }
}