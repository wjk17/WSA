using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa
{
    public static class SystemTool
    {
        public static void SafeInvoke(this Action action)
        {
            if (action != null) action.Invoke();
        }
    }
}