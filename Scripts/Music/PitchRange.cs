using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using System;
    [Serializable]
    public class PitchRange // 音域（2个音符之间）
    {
        public Note this[int i]
        {
            get { return range[i]; }
        }
        public PitchRange(params Note[] notes)
        {
            this.range = notes;
        }
        public int Pitchs
        {
            get { return max.pitch - min.pitch + 1; }
        }
        public int Scales
        {
            get { return max.scale - min.scale + 1; }
        }
        public Note min { get { return range[0]; } }
        public Note max { get { return range[1]; } }
        public Note[] range;
    }
}