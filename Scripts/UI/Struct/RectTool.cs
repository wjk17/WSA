using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa._UI
{
    public static class RTTool
    {
        public static Vector2 ParentPivotV(this RectTransform RT)
        {
            return ParentSizeAnchored(RT) * RT.pivot;
        }
        public static Vector2 ParentSizeAnchored(this RectTransform RT)
        {
            var parentSizeAbs = ParentSizeAbs(RT);
            return parentSizeAbs * RT.anchorMax - parentSizeAbs * RT.anchorMin;
        }
        public static Vector2 AnchorMinV(this RectTransform RT)
        {
            return RT.ParentSizeAbs() * RT.anchorMin;
        }
        public static Vector2 ParentSizeAbs(this RectTransform RT)
        {
            return RT.parent.GetComponent<Canvas>() != null ? // root
                UI.scalerRefRes : (RT.parent as RectTransform).rect.size;
        }
        public static Vector2 GetLBPos(this RectTransform RT)
        {
            var pos = RT.anchoredPosition;
            var size = RT.rect.size;
            var pivot = RT.pivot;
            var anchorMinV = AnchorMinV(RT);
            return pos - (size * pivot - ParentPivotV(RT)) + anchorMinV;
        }
    }
}