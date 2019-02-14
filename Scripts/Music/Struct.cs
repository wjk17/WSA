using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Esa
{
    using System;
    public enum Pitch // 音高 (12平均律 0~11)
    {
        C,      // 1
        CSharp, // #1 (b2)
        D,      // 2
        DSharp, // #2 (b3)
        E,      // 3
        F,      // 4
        FSharp, // #4 (b5)
        G,      // 5
        GSharp, // #5 (b6)
        A,      // 6
        ASharp, // #6 (b7)
        B,      // 7
    }
    [Serializable]
    public class Note // 音符
    {
        public Note sclMin { get { return new Note(scale, Pitch.C); } }
        public Note sclMax { get { return new Note(scale, Pitch.B); } }
        public override bool Equals(object obj)
        {
            var n = (Note)obj;
            return scale == n.scale && pitch == n.pitch;
        }
        public override int GetHashCode()
        {
            return scale.GetHashCode() ^ pitch.GetHashCode();
        }
        public Note SetScale(int scale)
        {
            return new Note(scale, pitch);
        }
        public Note(int scale, Pitch pitch)
        {
            this.scale = scale;
            this.pitch = pitch;
        }
        public int scale; // 音阶
        public Pitch pitch;
        string ToStr(Pitch p)
        {
            switch (p)
            {
                case Pitch.C: return "C";
                case Pitch.CSharp: return "C#";
                case Pitch.D: return "D";
                case Pitch.DSharp: return "D#";
                case Pitch.E: return "E";
                case Pitch.F: return "F";
                case Pitch.FSharp: return "F#";
                case Pitch.G: return "G";
                case Pitch.GSharp: return "G#";
                case Pitch.A: return "A";
                case Pitch.ASharp: return "A#";
                case Pitch.B: return "B";
                default: throw new Exception();
            }
        }
        string ToStrNum(Pitch p)
        {
            switch (p)
            {
                case Pitch.C: return "1";
                case Pitch.CSharp: return "#1";
                case Pitch.D: return "2";
                case Pitch.DSharp: return "#2";
                case Pitch.E: return "3";
                case Pitch.F: return "4";
                case Pitch.FSharp: return "#4";
                case Pitch.G: return "5";
                case Pitch.GSharp: return "#5";
                case Pitch.A: return "6";
                case Pitch.ASharp: return "#6";
                case Pitch.B: return "7";
                default: throw new Exception();
            }
        }
        public string ToString2()
        {
            return ToStr(pitch).ToLower() + scale.ToString();
        }
        public override string ToString()
        {
            //return ToStr(pitch) + scale.ToString();
            return ToStrNum(pitch);
        }
    }
    [Serializable]
    public class Beat // 拍子
    {
        public List<Note> notes;
    }
    [Serializable]
    public class Bar // 小节
    {
        public List<Beat> beats;
    }
    [Serializable]
    public class Para // 段落
    {
        public List<Bar> bars;
    }
}
