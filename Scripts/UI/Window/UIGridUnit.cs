using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    using System;
    [Serializable]
    public class GridUnitProp
    {
        [NonSerialized]
        public Rect rect;
        public bool clickable = true;
        public bool visible = true;
        public string name = "";
        public Texture2D texture = null;
        public bool Hover()
        {
            return rect.Contains(UI.mousePosRef) && clickable;
        }

        internal void DrawTexture()
        {
            GLUI.DrawTex(texture, rect.ToPointsCWLT(-1));
        }

        internal void DrawName(Vector2 os)
        {            
            GLUI.DrawString(name, (rect.pos + os), Vectors.half2d);
        }
    }
}