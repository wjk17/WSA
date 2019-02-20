using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Esa
{
    using System;

    [Serializable]
    public class Chord
    {
        public List<NoteMulti> notes;
        public Chord()
        {
            notes = new List<NoteMulti>();
        }
        public void Append()
        {
            notes.Append();
        }
        public void Repeat()
        {
            notes.Repeat();
        }
    }
    [Serializable]
    public class Beat // 拍子
    {
        public List<NoteVal> notes;
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
    [Serializable]
    public class Lyric
    {
        public Lyric() { strs = new List<string>(); }
        public string this[int i]
        {
            get { return strs[i]; }
        }
        public List<string> strs;
        public bool multiChars;
        internal void NewWord()
        {
            strs.Add("");
        }
        internal void Append(char v)
        {
            if (multiChars)
                strs[strs.Count - 1] += v;
            else
                strs.Add(v.ToString());
        }
    }
    [Serializable]
    public class LinkLine
    {
        public LinkLine(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
        public int start;
        public int end;
    }
}
