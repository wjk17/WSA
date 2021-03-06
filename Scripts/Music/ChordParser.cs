﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using System;
    [Serializable]
    public class ChordParser
    {
        public List<Chord> chords;
        public Chord chord { get { return chords.Last(); } }
        public NoteMulti noteMulti { get { return chord.notes.Last(); } }
        public NoteMulti peek { get { return chord.notes.Last_2(); } }
        public Note note { get { return noteMulti.notes.Last(); } }

        int scale;
        int pitchShift;
        public int beatPerNote = 4; // X/beatPerNote 拍，一个全音符占几拍
        float value;

        bool multiNote;

        string str;
        bool comment;
        bool arrange;
        public List<int> chordArr;
        public int baseScale = 4;

        public ChordParser()
        {
            chords = new List<Chord>();
            chords.Append();

            value = 1f / beatPerNote; // 时值

            chord.Append();
            noteMulti.Append();
            noteMulti.value = value;

            chordArr = new List<int>();

            scale = baseScale;
            pitchShift = 0;

            multiNote = false;

            str = "";
            comment = false;
            arrange = false;
        }
        bool ReadNums(char c)
        {
            switch (c)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    str += c;
                    if (!arrange) DealNums();
                    return true;

                default:
                    if (str.Length > 0)
                        DealNums();
                    return false;
            }
        }

        private void DealNums()
        {
            int i = int.Parse(str);
            str = "";
            if (arrange)
            {
                chordArr.Add(i);
            }
            if (i == 0) return;//休止符暂不支持
            int pitch = PitchTool.Pitch8_to_12(i - 1);
            note.scale = scale;
            note.pitch = (Pitch)(pitchShift + pitch);
            pitchShift = 0;

            if (multiNote)
            {
                noteMulti.Append(); // 往多音音符里 添加新的单音
            }
            else//!multiNote
            {
                chord_Append();
            }
        }
        void chord_Append()
        {
            pitchShift = 0;

            chord.Append(); // 往和弦里 添加新的单音
            noteMulti.Append();
            noteMulti.value = value;
        }

        internal void ClearEmpty()
        {
            var n = new List<Chord>();
            foreach (var chord in chords)
            {
                chord.ClearEmpty();
                if (chord.notes.Count != 0) n.Add(chord);
            }
            chords = n;
        }
        public void Read(char c)
        {
            if (c == '\\' || c == '/') { comment = !comment; return; }
            if (comment) return;

            if (ReadNums(c)) return;
            if (arrange) return;
            switch (c)
            {
                case '@':
                    arrange = !arrange;
                    break;

                case '<':
                    multiNote = true;
                    break;

                case '>':
                    multiNote = false;
                    chord_Append(); // 把多音音符 添加到和弦
                    break;

                case '_': // 时值减半 Half Value
                    noteMulti.value *= 0.5f;
                    break;
                case '.':
                    peek.value *= 1.5f;
                    break;
                case '-':
                    peek.value += 1f / beatPerNote;
                    break;

                case ' ':
                    break;

                case ',':
                    chords.Append();

                    chord.Append();
                    noteMulti.Append();
                    noteMulti.value = value;

                    break;

                case '$':
                    chord.ClearEmpty();
                    chord.Repeat();

                    chord.Append();
                    noteMulti.Append();
                    noteMulti.value = value;
                    break;

                case '(': // Flat
                    scale--;
                    break;
                case ')': //
                    scale++;
                    break;

                case '[': // Sharp
                    scale++;
                    break;
                case ']': //
                    scale--;
                    break;


                case 'b':
                    pitchShift = -1;
                    break;
                case '#':
                    pitchShift = 1;
                    break;

                case '\r':
                case '\n':
                    break;

                default:
                    throw new Exception("undef Para Character " + c + ".");
            }
        }
    }
}