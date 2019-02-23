using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    public class UIConsole : MonoBehaviour
    {
        public bool visible;
        public KeyCode hotkey = KeyCode.H;
        void Start()
        {
            this.AddInput(Input, 0, true);
        }
        void Input()
        {
            if (Events.KeyDown(hotkey)) visible = !visible;
            this.BeginOrtho();
            if (!visible) return;
            
            this.DrawBG();
            var rt = new RectTrans(this);
            var center = (rt.cornerLT + rt.cornerRT) * 0.5f;
            var pos = center - 15.Y();
            GLUI.DrawString("控制台(" + hotkey.ToString() + ")", pos, Vectors.half2d);

            var buttonSize = new Vector2(150, 45);
            if (EG.Button("遇战", center - 80.Y(), buttonSize))
            {
                Battle._Start();
            }
            else if (EG.Button("满状态", center - 140.Y(), buttonSize))
            {
                CharCtrl.I.P.hp = CharCtrl.I.P.hpMax;
                CharCtrl.I.P.mp = CharCtrl.I.P.mpMax;
            }
            //if (EG.Button("测试命令", center - 120.Y(), buttonSize))
            //{
            //    SkillMgr.GetSkill("flee").Cast();
            //}
            else if (EG.Button("X", rt.cornerRT - 24.XY() + 2.Y(), 24.XY()))
            {
                visible = !visible;
            }
            else this.DoDragWindow(); // 可拖曳窗口
        }
    }
}