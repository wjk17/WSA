using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tool
{
    public static void swapPts(ref Vector2 p1, ref Vector2 p2)
    {
        var tmp = p1; p1 = p2; p2 = tmp;
    }
    public static void swapCodes(ref byte c1, ref byte c2)
    {
        var tmp = c1; c1 = c2; c2 = tmp;
    }
}
