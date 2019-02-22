using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using UI_;
    using System;
    public class ScoreGen : MonoBehaviour
    {
        public Para para { get { return loader.para; } }
        public ScoreLoader loader;
        public int barPerLine = 2; // 每行显示几个小节
        public int lineSpace = 5; // 行距
        public int beatSpaceAdd = 2;
        public int noteSpaceAdd = 2; // 音符间隔 （额外的）
        public int fontSize = 32; // 音符字体大小
        public int fontSizeLyric = 24; // 音符字体大小
        public int fontSizeShift = 16; // 升降记号字体大小
        public int fontSizeH1 = 32; // 标题（歌名）
        public int fontSizeH2 = 24; // 副标题（演唱者）
        public Vector2 h2Os;
        public Vector2 bodyOs;
        public Vector2 shiftOSNor = Vector2.one * -0.3f;
        public Vector2 margin = Vector2.one * 50;
        public int middleScale5;

        public string shiftMark = "·";
        public int shiftMarkFontSize = 32;
        public float shiftMarkOSFactor = 0.8f;
        public float shiftMarkOS = 0.65f;

        void Start()
        {
            this.AddInput(Input, 2, false);
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
        public Color passFontColor = Color.blue;
        public int passIdx;
        public bool drawLyric;

        void Input()
        {
            this.BeginOrtho(-2);
            GLUI.SetFontColor(Color.black);
            this.Draw(Palette.L8, true);
            this.Draw(Color.black, false);
            var rt = new RectTrans(this);
            var basePos = rt.centerT + Vector2.down * margin.y;


            // title 
            var fn = loader.fileName.Split('.')[0];
            var ss = fn.Split('-');

            var songName = ss[1];
            GLUI.DrawString(songName, basePos, fontSizeH1, Vector2.up + Vectors.halfRight2d);

            var singerName = ss[0];
            GLUI.DrawString("演唱: " + singerName, basePos + h2Os, fontSizeH2, Vector2.up + Vectors.halfRight2d);
            basePos += Vector2.down * bodyOs;

            // average
            float barWidth = (rt.sizeAbs.x - margin.x * 2) / barPerLine;
            var psBar = basePos.Average(barWidth, barPerLine, Vectors.halfRight);

            int noteIdx = -1;
            //var notes = loader.notesTotal;
            //for (int i = 0; i < notes.Count; i++)
            //{
            //    var x = i % barPerLine;
            //    var y = i / barPerLine;
            //    var posBar = psBar[x] + Vector2.down * y * (lineSpace + fontSize);
            //    var psNote = posBar.Average(barWidth, barPerLine, Vectors.halfRight);

            //    // 弹过的音符变色
            //    GLUI.SetFontColor(Color.black);
            //    noteIdx++;
            //    if (passIdx >= noteIdx)
            //        GLUI.SetFontColor(passFontColor);

            //    var note = notes[i];
            //    var str = note.ToString().Reverse(); ;
            //    if (str.Length == 2)
            //    {
            //        GLUI.DrawString(str[1], psNote[x] + fontSize * shiftOSNor, fontSizeShift, Vector2.up + Vectors.halfRight2d);
            //    }
            //    GLUI.DrawString(str[0], psNote[x], fontSize, Vector2.up + Vectors.halfRight2d);

            //    // 变调小圆点
            //    var scaleShift = note.scale - middleScale5;
            //    var factor = Mathf.Sign(scaleShift);
            //    scaleShift = Mathf.Abs(scaleShift);
            //    for (int m = 0; m < scaleShift; m++)
            //    {
            //        var dir = Vector2.up * factor;
            //        GLUI.DrawString(shiftMark,
            //            psNote[x] + Vector2.down * fontSize * 0.5f +
            //            dir * fontSize * shiftMarkOS +
            //            dir * shiftMarkFontSize * m * shiftMarkOSFactor,
            //            fontSize, Vectors.half2d);

            //    }
            //}

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
                        GLUI.SetFontColor(Color.black);
                        noteIdx++;
                        if (passIdx >= noteIdx)
                            GLUI.SetFontColor(passFontColor);
                        var note = notes[k];
                        var str = note.ToString().Reverse(); ;
                        if (str.Length == 2)
                        {
                            GLUI.DrawString(str[1], psNote[k] + fontSize * shiftOSNor, fontSizeShift, Vector2.up + Vectors.halfRight2d);
                        }
                        GLUI.DrawString(str[0], psNote[k], fontSize, Vector2.up + Vectors.halfRight2d);

                        if (drawLyric)
                        {
                            // 歌词
                            var word = loader.lyric[noteIdx];
                            var sp = Vectors.halfDown2d * (lineSpace + fontSize);
                            var pos = psNote[k] + sp;
                            GLUI.DrawString(word, pos, fontSizeLyric, Vector2.up + Vectors.halfRight2d);
                        }
                        // 变调小圆点
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