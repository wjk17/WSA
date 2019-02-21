using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using System;
    using System.IO;
    using UI_;
    using ENC = System.Text.Encoding;
    public enum Encoding
    {
        UTF8,
        UTF7,
        UTF32,
        Unicode,
        BigEndianUnicode,
        ASCII,
        Default
    }
    public static class TextTool
    {
        public static ENC ToENC(this Encoding encoding)
        {
            switch (encoding)
            {
                case Encoding.UTF8: return ENC.UTF8;
                case Encoding.UTF7: return ENC.UTF7;
                case Encoding.UTF32: return ENC.UTF32;
                case Encoding.Unicode: return ENC.Unicode;
                case Encoding.BigEndianUnicode: return ENC.BigEndianUnicode;
                case Encoding.ASCII: return ENC.ASCII;
                case Encoding.Default: return ENC.Default;
                default: throw new Exception();
            }
        }
    }
    public class ScoreLoader : MonoBehaviour
    {
        public string filePath;
        public string folder = @"\Score\";
        public string fileName = ".txt";
        public Encoding encoding = Encoding.UTF8;
        //[TextArea(1, 50)] 
        public string text;
        public int step;
        public bool readAllOnStart = true;
        public ChordParser chordParser;

        [Button]
        void GetPath()
        {
            filePath = Application.streamingAssetsPath + folder + fileName;
        }

        [Button]
        public void Start()
        {
            var enc = encoding.ToENC();
            if (File.Exists(filePath))
            {
                text = File.ReadAllText(filePath, enc);
                InitValues();
                if (readAllOnStart) ReadAll();
            }
        }
        [Button]
        void ReadStep()
        {
            if (step < text.Length)
                ReadParaChar(text[step++]);
        }
        [Button]
        void ReadAll()
        {
            foreach (var c in text)
            {
                if (chordMode)
                    chordParser.Read(c);
                else ReadParaChar(c);
            }
            chordParser.ClearEmpty();

            UIChord.I.names = new string[chords.Count];
            for (int i = 0; i < chords.Count; i++)
            {
                UIChord.I.names[i] = (i + 1).ToString();
            }
        }
        [Button]
        void InitValues()
        {
            var gen = FindObjectOfType<ScoreGen>();
            gen.passIdx = -1;

            chordParser = new ChordParser();
            var kb = FindObjectOfType<InstKeyboard>();
            kb.chordIdx = 0;

            UIChord.I.idx = -1;

            step = 0;

            notes = new List<NoteVal>();
            notesTotal = new List<NoteVal>();

            beat = new Beat();
            beats = new List<Beat>();

            bar = new Bar();
            bars = new List<Bar>();

            para = new Para();
            para.bars = bars;

            lines = new List<LinkLine>();
            linesStack = new Stack<LinkLine>();

            scale = scaleMid;
            pitch = 0;
            value = valueDivideNum;

            lyric = new Lyric();
            lyricMode = false;
            chordMode = false;

            ppShifts = new List<PPShift>();
            pitchParaShift = 0;                    
        }
        public Para para;
        public List<NoteVal> notesTotal;
        [SerializeField] List<NoteVal> notes;

        [SerializeField] List<Beat> beats;
        [SerializeField] Beat beat;

        [SerializeField] List<Bar> bars;
        [SerializeField] Bar bar;

        int scale;
        int scaleMid = 5;
        int pitch;
        int value;

        int tune; // 曲调
        int beatPerBar = 4; // 每个小节有几拍
        int valueDivideNum = 4; // 每个音符的时值是 几分之一拍

        public List<LinkLine> lines;
        public Stack<LinkLine> linesStack;

        public List<Chord> chords { get { return chordParser.chords; } }

        public Lyric lyric;
        public bool lyricMode;

        public bool chordMode;

        public int pitchParaShift;
        public bool pitchParaShifted = true;
        [Serializable]
        public struct PPShift
        {
            public PPShift(int idx, int shift)
            {
                this.idx = idx;
                this.shift = shift;
            }
            public int idx;
            public int shift;
        }
        public List<PPShift> ppShifts;
        private void ReadParaChar(char c)
        {
            switch (c)
            {
                case 'B': // 如果谱是已经降了的，升回来到原调
                    if (pitchParaShifted) pitchParaShift++;
                    ppShifts.Add(new PPShift(notesTotal.Count, -1));
                    break;

                case 'S': // 相反
                    if (pitchParaShifted) pitchParaShift--;
                    ppShifts.Add(new PPShift(notesTotal.Count, 1));
                    break;

                case '@':
                    chordMode = true;
                    break;

                case '_': // 时值减半 Half Value
                    if (lyricMode)
                        lyric.Append(' ');
                    else value *= 2;
                    break;

                case ' ': // Beat
                    if (lyricMode)
                    {
                        if (lyric.multiChars) lyric.NewWord();
                        else break;
                    }
                    else
                    {
                        if (notes.Count == 0) break;
                        beat.notes = notes;
                        notes = new List<NoteVal>();

                        beats.Add(beat);
                        beat = new Beat();
                    }
                    break;

                case ',': // Bar
                    if (notes.Count == 0) break;

                    ReadParaChar(' ');

                    bar.beats = beats;
                    beats = new List<Beat>();

                    bars.Add(bar);
                    bar = new Bar();
                    break;

                case '.':
                    break;
                case '-':
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

                case '{': // Link
                    var line = new LinkLine(notesTotal.Count, 0);
                    linesStack.Push(line);
                    lines.Add(line);
                    break;
                case '}': //
                    linesStack.Pop().end = notesTotal.Count - 1;
                    break;

                case 'b':
                    pitch = -1;
                    break;
                case '#':
                    pitch = 1;
                    break;

                case '0':
                    break;//休止符暂不支持
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                    int i = PitchTool.Pitch8_to_12(int.Parse(c.ToString()) - 1);
                    var n = new NoteVal(scale, (Pitch)(pitch + i), value);
                    n += pitchParaShift;
                    notes.Add(n);
                    notesTotal.Add(n);

                    pitch = 0;
                    value = valueDivideNum;
                    break;

                case '\r':
                case '\n': // end lyric
                    lyricMode = false;
                    lyric.multiChars = false;
                    break;

                case '\\': // start lyric
                    lyricMode = true;
                    break;

                case '*':
                    if (lyricMode)
                    {
                        lyric.multiChars = true;
                        lyric.NewWord();
                    }
                    else goto default;
                    break;

                default:
                    if (lyricMode)
                        lyric.Append(c);
                    else throw new Exception("undef Para Character " + c + ".");
                    break;
            }
        }
    }
}