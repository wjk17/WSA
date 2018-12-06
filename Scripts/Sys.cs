using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Esa
{
    public class Sys
    {
        public static float Fps
        {
            get { return fps; }
            set { fps = value; }
        }
        static float fps;
        public static float Tpf // timePerFrame
        {
            get { return 1 / Fps; }
        }
    }
}
