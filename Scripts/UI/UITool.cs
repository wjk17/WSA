using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI
{
    public static partial class UITool
    {
        public static Rt GetRt(this RectTransform rt)
        {
            // anchor 和 pivot 都要上下翻转，转成左上角坐标
            var pos = rt.anchorMin.SubY_L(1) * UI.scaler.referenceResolution;
            pos += rt.anchoredPosition.ReverseY();
            pos += -rt.pivot.SubY_L(1) * rt.rect.size;
            return new Rt(pos, rt.rect.size);
        }
        public static Vector2 ToLT(this Vector2 pos) // input screen pos
        {
            pos.y = UI.scaler.referenceResolution.y - pos.y;
            return pos;
        }
        public static Vector2 ToRefLT(this Vector2 pos) // input screen pos
        {
            pos *= UI.facterToReference;
            pos.y = UI.scaler.referenceResolution.y - pos.y;
            return pos;
        }
    }
}