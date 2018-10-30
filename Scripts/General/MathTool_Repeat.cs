using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MathTool {
    public static int RepeatAbs(this int t, int length)
    {
        var mod = t % length;
        return mod == 0 ? 0 : (t < 0 ? length + mod : mod);
    }
}
