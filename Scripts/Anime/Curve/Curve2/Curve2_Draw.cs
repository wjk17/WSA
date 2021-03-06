﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
namespace Esa
{
    public partial class Curve2
    {
        public delegate void ActDrawLine(Vector2 a, Vector2 b, Color color, Matrix4x4 m);
        public delegate void ActDrawPoint(Vector2 p, Color color, Matrix4x4 m);
        public static ActDrawLine drawLine;
        public static ActDrawPoint drawVector;
        public static ActDrawPoint drawTangent;
        public static Color colorTrack;
        public static Color colorCtrlLines;
        public static Color colorVectors;
        public static Color colorTangents;
        public static Color colorBorder;
        public static int accuracy = 100;
        //在界面显示的区域，暂时不保存。
        [XmlIgnore] public Vector2 drawAreaSize = Vector2.one;
        [XmlIgnore] public Vector2 drawAreaOffset = Vector2.zero;
        internal void Draw(Matrix4x4 m, bool showTangents)
        {
            //grid 0, border 1, axis 2, lines 3, points 4, timeline 5
            UI_.GLUI.BeginOrder(1);
            DrawBorder(m);
            UI_.GLUI.BeginOrder(3);
            DrawLines(m, showTangents);
            UI_.GLUI.BeginOrder(4);
            DrawPoints(m, showTangents);
        }

        private void DrawBorder(Matrix4x4 m)
        {
            var lt = drawAreaOffset + drawAreaSize.Y();
            var lb = drawAreaOffset;
            var rb = drawAreaOffset + drawAreaSize.X();
            var rt = drawAreaOffset + drawAreaSize;
            drawLine(lt, rt, colorBorder, m);
            drawLine(lt, lb, colorBorder, m);
            drawLine(rt, rb, colorBorder, m);
            drawLine(lb, rb, colorBorder, m);
        }

        public void DrawPoints(Matrix4x4 m, bool showTangents)
        {
            foreach (var key in keys)
            {
                drawVector(key.frameKey, colorVectors, m);
                if (showTangents && key.inMode == KeyMode.Bezier) drawTangent(key.inKey, colorTangents, m);
                if (showTangents && key.outMode == KeyMode.Bezier) drawTangent(key.outKey, colorTangents, m);
            }
        }
        public void DrawLines(Matrix4x4 m, bool showTangents)
        {
            if (keys == null || Count == 0) return;
            // 根据显示区域用Evaluate画出局部曲线
            var factor = drawAreaSize.x / (accuracy - 1);
            var prev = new Vector2(0f, keys[0].value);
            float t;
            Vector2 curr;
            for (int i = 1; i < accuracy; i++)
            {
                t = drawAreaOffset.x + i * factor;
                if (time1D)
                {
                    curr = new Vector2(t, Evaluate1D(t));
                }
                else
                {
                    curr = Evaluate2D(t);
                }
                drawLine(prev, curr, colorTrack, m);
                prev = curr;
            }

            foreach (var key in keys)
            {
                if (showTangents && key.inMode == KeyMode.Bezier) drawLine(key.frameKey, key.inKey, colorCtrlLines, m);
                if (showTangents && key.outMode == KeyMode.Bezier) drawLine(key.frameKey, key.outKey, colorCtrlLines, m);
            }
        }
    }
}