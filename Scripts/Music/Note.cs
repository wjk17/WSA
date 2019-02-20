using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Esa
{
    using System;
    [Serializable]
    public class NoteMultiDep // 多音音符，时值独立
    {
        public List<NoteVal> notes;
    }
    [Serializable]
    public class NoteMulti : ICloneable // 多音音符，时值统一
    {
        public float value;
        public List<Note> notes;
        public NoteMulti()
        {
            notes = new List<Note>();
        }
        public NoteMulti(List<Note> notes)
        {
            this.notes = notes;
        }
        public void Append()
        {
            notes.Append();
        }

        public object Clone()
        {
            return new NoteMulti(notes.MemsClone());
        }
    }
    [Serializable]
    public class NoteVal : Note // 音符（带时值）
    {
        public int value;
        public NoteVal() : base() { }
        public NoteVal(int scale, Pitch pitch, int value) : this(scale, pitch)
        {
            this.value = value;
        }
        public NoteVal(int scale, Pitch pitch) : base(scale, pitch)
        {
        }
    }
    [Serializable]
    public class Note : ICloneable// 音符
    {
        public object Clone()
        {
            return new Note(scale, pitch);
        }
        public Note sclMin { get { return new Note(scale, Pitch.C); } }
        public Note sclMax { get { return new Note(scale, Pitch.B); } }
        public static int operator -(Note a, Note b)
        {
            return (a.scale - b.scale) * 12 + (a.pitch - b.pitch);
        }
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
        public Note()
        {

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
                default: throw new Exception("Undef Pitch: " + p.ToString());
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
                case Pitch.Rest: return "0";
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
}
