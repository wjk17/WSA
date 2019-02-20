using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using UI_;
    using System;
    public class InstKeyboard : MonoBehaviour
    {
        public List<UIGrid_2_0> grids;
        public InstSource src;
        public PianoMeshCtrl piano;
        public int scaleCurr;
        public int scaleMid = 5;
        public KeyCode[] keys;
        public KeyCode[] keysLow;
        public KeyCode[] keysChord;
        private ScoreLoader loader;
        private ScoreGen gen;
        public int chordIdx;
        public ChordRT chordPlaying;
        public int beatPerMinute = 120;
        public float secondPerNote { get { return 60f / beatPerMinute; } }
        [Button]
        void SetKeysChord()
        {
            keysChord = new KeyCode[]
            {
                KeyCode.A,
                KeyCode.S,
                KeyCode.D,
                KeyCode.F,
                KeyCode.G,
                KeyCode.H,
                KeyCode.J,
                KeyCode.K,
                KeyCode.L,
                KeyCode.Colon,

                KeyCode.Z,
                KeyCode.X,
                KeyCode.C,
                KeyCode.V,
                KeyCode.B,
                KeyCode.N,
                KeyCode.M,
                KeyCode.Comma,
                KeyCode.Period,
                KeyCode.Slash,
            };
        }
        [Button]
        void SetKeys()
        {
            keys = new KeyCode[] {
                KeyCode.Q,
                KeyCode.Alpha2,
                KeyCode.W,
                KeyCode.Alpha3,
                KeyCode.E,

                KeyCode.R,
                KeyCode.Alpha5,
                KeyCode.T,
                KeyCode.Alpha6,
                KeyCode.Y,
                KeyCode.Alpha7,
                KeyCode.U,
                KeyCode.I,
            };
            keysLow = new KeyCode[] {
                KeyCode.Z,
                KeyCode.S,
                KeyCode.X,
                KeyCode.D,
                KeyCode.C,

                KeyCode.V,
                KeyCode.G,
                KeyCode.B,
                KeyCode.H,
                KeyCode.N,
                KeyCode.J,
                KeyCode.M,
                KeyCode.Comma,
            };
        }
        [Button]
        void GetGrids()
        {
            grids = this.GetComChildren<UIGrid_2_0>();
        }
        void Start()
        {            
            loader = FindObjectOfType<ScoreLoader>();
            gen = FindObjectOfType<ScoreGen>();
            grids[0].onClick = (i) => PlayPitch(PitchTool.Pitch8_to_12(i));
            grids[1].onClick = (i) => PlayPitch(PitchTool.Pitch5_to_12(i));
            this.AddInput(Input, -1, false);
        }
        private void Input()
        {
            if (Events.Shift) scaleCurr = scaleMid + 1;
            else if (Events.Ctrl) scaleCurr = scaleMid - 1;
            else scaleCurr = scaleMid;
            src.UnHold();
            foreach (var grid in grids)
            {
                foreach (var gup in grid.gridUnitProp)
                {
                    gup.clickable = true;
                }
            }
            piano.notePress = new List<Note>();
            for (int i = 0; i < keys.Length; i++)
            {
                var idx = PitchTool.Pitch12To8_5_Align(i);
                var grid = idx > 0 ? 0 : 1;
                if (Events.KeyDown(keys[i]))
                {
                    PlayPitch(i);
                    grids[grid].gridUnitProp[Mathf.Abs(idx) - 1].clickable = false;
                }
                if (Events.Key(keys[i]))
                {
                    grids[grid].gridUnitProp[Mathf.Abs(idx) - 1].clickable = false;
                    HoldPitch(i);
                }
            }
            // 支持同时按下的按键数量取决键盘设备（的电路设计？），一般为2~5个键，
            // 不包括Shift和Ctrl，这类特殊键一般可以额外同时按下。
            // A~Z字母键能确保2个键同时按下，3个及以上就“不一定”了。
            // 据说专门的游戏可以支持更多键。
            // 比如我的键盘可以同时按 QRXC 四个键， 但按不了 QRV、QRB。
            scaleCurr--;
            for (int i = 0; i < keysLow.Length; i++)
            {
                if (Events.KeyDown(keysLow[i]))
                {
                    PlayPitch(i);
                }
                if (Events.Key(keysLow[i]))
                {
                    HoldPitch(i);
                }
            }
            for (int i = 0; i < keysChord.Length; i++)
            {
                if (Events.KeyDown(keysChord[i]))
                {
                    PlayChord(i);
                }
            }

            if (Events.KeyDown(KeyCode.Space))
            {
                chordIdx++;
                if (chordIdx > 0 && chordIdx - 1 < loader.chordParser.chordArr.Count)
                {
                    PlayChordArr(chordIdx - 1);
                }
            }

            if (chordPlaying != null && chordPlaying.notes.NotEmpty())
            {
                chordPlaying.Update();
                if (chordPlaying.t >= chordPlaying.endTime.Last())
                    chordPlaying = null;
            }
        }
        private void PlayChordArr(int i)
        {
            PlayChord(loader.chordParser.chordArr[i] - 1);
        }
        private void PlayChord(int idx)
        {
            var chord = new ChordRT(loader.chords[idx], secondPerNote);
            chord.playNote = PlayPitch;
            chord.holdNote = HoldPitch;
            chordPlaying = chord;
        }
        [Serializable]
        public class ChordRT : Chord
        {
            public ChordRT(Chord chord, float secondPerNote)
            {
                notes = chord.notes;
                pressed = new bool[notes.Count];
                endTime = new float[notes.Count];
                startTime = new float[notes.Count];
                float start = 0f;
                for (int i = 0; i < notes.Count; i++)
                {
                    startTime[i] = start * secondPerNote;
                    endTime[i] = start + notes[i].value;
                    endTime[i] *= secondPerNote;

                    start += notes[i].value;
                }

            }
            public bool[] pressed;
            public float[] endTime;
            public float[] startTime;
            public Action<NoteMulti> playNote;
            public Action<NoteMulti> holdNote;
            public float t;
            public void Update()
            {
                t += Time.deltaTime;
                for (int i = 0; i < notes.Count; i++)
                {
                    if (!pressed[i] && t >= startTime[i])
                    {
                        pressed[i] = true;
                        playNote(notes[i]);
                        return;
                    }
                    if (t < endTime[i])
                    {
                        holdNote(notes[i]);
                        return;
                    }
                }
            }
        }
        void PlayPitch(NoteMulti note)
        {
            foreach (var n in note.notes)
            {
                src.Play(n);
            }
        }
        void HoldPitch(NoteMulti note)
        {
            foreach (var n in note.notes)
            {
                src.Hold(n);
                piano.notePress.Add(n);
            }
        }
        void HoldPitch(int i)
        {
            var note = GetPitch(i);
            src.Hold(note);
            piano.notePress.Add(note);
        }
        void PlayPitch(int i)
        {
            var n = GetPitch(i);

            if ((gen.passIdx + 1) < loader.notesTotal.Count
                && n.Equals(loader.notesTotal[gen.passIdx + 1]))
                gen.passIdx++;

            src.Play(n);
        }
        Note GetPitch(int i)
        {
            var scl = scaleCurr + i / 12;
            var pitch = i % 12;
            return new Note(scl, (Pitch)pitch);
        }
    }
}