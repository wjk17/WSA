using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using UI_;
    using System;
    public class ScoreGen : MonoBehaviour
    {
        public Para para;
        public int barPerLine = 2; // 每行显示几个小节
        public int lineSpace = 5; // 行距
        public int beatSpaceAdd = 2;
        public int noteSpaceAdd = 2; // 音符间隔 （额外的）
        public int fontSize = 32; // 音符字体大小
        public int fontSizeShift = 16; // 升降记号字体大小
        public Vector2 shiftOSNor = Vector2.one * -0.3f;
        public Vector2 margin = Vector2.one * 50;
        public int middleScale5;

        public string shiftMark = "·";
        public int shiftMarkFontSize = 32;
        public float shiftMarkOSFactor = 0.8f;
        public float shiftMarkOS = 0.65f;

        void Start()
        {
            this.AddInput(Input, 0);
            var fontColor = Color.black;
            GLUI.fontColor = fontColor;
            GLUI.font.material.SetColor("_Color", fontColor);

            //para.bars = new List<Bar>();
            //for (int i = 0; i < 5; i++)
            //{
            //    var bar = new Bar();
            //    bar.beats = new List<Beat>();

            //    for (int j = 0; j < 4; j++)
            //    {
            //        var beat = new Beat();
            //        beat.notes = new List<Note>();

            //        for (int k = 0; k < 1; k++)
            //        {
            //            var note = new Note();
            //            beat.notes.Add(note);
            //        }
            //        bar.beats.Add(beat);
            //    }

            //    para.bars.Add(bar);
            //}
        }
        [Button]
        void SetScaleToMiddle()
        {
            foreach (var bar in para.bars)
            {
                foreach (var beat in bar.beats)
                {
                    foreach (var note in beat.notes)
                    {
                        note.scale = 5;
                    }
                }
            }
        }
        public int textIdx;
        public string str;
        void Input()
        {
            this.BeginOrtho();
            this.Draw(Palette.L8, true);
            var rt = new RectTrans(this);
            float barWidth = (rt.sizeAbs.x - margin.x * 2) / barPerLine;
            var basePos = rt.centerT + Vector2.down * margin.y;
            var psBar = basePos.Average(barWidth, barPerLine, Vectors.halfRight);

            str = GetComponent<ScoreLoader>().text[textIdx].ToString();
            GLUI.DrawString(str, Vector2.zero);

            for (int i = 0; i < para.bars.Count; i++)
            {
                var bar = para.bars[i];
                var beats = bar.beats;
                var x = i % barPerLine;
                var y = i / barPerLine;

                var posBar = psBar[x] + Vector2.down * y * (lineSpace + fontSize);

                var beatWid = beatSpaceAdd + barWidth / beats.Count;
                var psBeat = posBar.Average(beatWid, beats.Count, Vectors.halfRight);

                for (int j = 0; j < bar.beats.Count; j++)
                {
                    var beat = beats[j];
                    var notes = beat.notes;

                    var noteWid = noteSpaceAdd + beatWid / notes.Count;
                    var psNote = psBeat[j].Average(noteWid, notes.Count, Vectors.halfRight);
                    for (int k = 0; k < notes.Count; k++)
                    {
                        var note = notes[k];
                        var str = note.ToString().Reverse(); ;
                        if (str.Length == 2)
                        {
                            GLUI.DrawString(str[1], psNote[k] + fontSize * shiftOSNor, fontSizeShift, Vector2.up + Vectors.halfRight2d);
                        }
                        GLUI.DrawString(str[0], psNote[k], fontSize, Vector2.up + Vectors.halfRight2d);

                        var scaleShift = note.scale - middleScale5;
                        var factor = Mathf.Sign(scaleShift);
                        scaleShift = Mathf.Abs(scaleShift);
                        for (int m = 0; m < scaleShift; m++)
                        {
                            var dir = Vector2.up * factor;
                            GLUI.DrawString(shiftMark,
                                psNote[k] + Vector2.down * fontSize * 0.5f +
                                dir * fontSize * shiftMarkOS +
                                dir * shiftMarkFontSize * m * shiftMarkOSFactor,
                                fontSize, Vectors.half2d);

                        }
                    }
                }
            }
        }
    }
}