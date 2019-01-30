using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    public static class EG
    {
        // 显示跳动的文字
        public static void Jumpy(string str, Color color)
        {

        }
        internal static void Blood()
        {

        }
        public static bool Button(string name, Vector2 pos, Vector2 buttonSize)
        {
            var rt = new Rect(pos, buttonSize);
            var down = false;
            if (Events.MouseDown0)
            {
                UITool.DrawButton(rt, Color.white, 2);
                down = rt.Contains(UI.mousePosRef); // down
            }
            else
            {
                if (rt.Contains(UI.mousePosRef)) // hover
                    UITool.DrawButton(rt, Color.white, 1);
                else
                    UITool.DrawButton(rt, Color.white, 0); // normal
            }
            GLUI.DrawString(name, pos, Vectors.half2d);
            return down;
        }
    }
}