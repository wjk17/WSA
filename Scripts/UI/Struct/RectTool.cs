using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTool
{
    public static Vector2 AnchoredPosLeftTop(this RectTransform rt)
    {
        return rt.anchoredPosition - rt.pivot * rt.rect.size;
    }
}
