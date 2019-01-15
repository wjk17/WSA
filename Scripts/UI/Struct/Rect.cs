using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa._UI;
namespace Esa
{
    /// <summary>
    /// Rect 默认锚点为中心
    /// </summary>
    [Serializable]
    public struct Rect
    {
        public Vector2 pos;
        public Vector2 size;
        public Vector2 pivot;
        public static implicit operator Rect(UnityEngine.Rect rect)
        {
            return new Rect(rect.position, rect.size);
        }
        public Rect(MonoBehaviour mono) : this(mono.transform as RectTransform)
        {
        }
        /// <summary>
        /// BUG：横向Strech模式下pivot.y必须为0，竖向则x为0，双向则都为0。
        /// </summary>
        public Rect(RectTransform rt)
        {
            pos = _UI.UI.AbsRefPos(rt);

            //pivot = rt.pivot.FlipY();
            pivot = Vectors.half2d;

            //pos -= rt.pivot.FlipY() * rt.rect.size;
            size = rt.rect.size;
        }
        public Rect(Vector2 pos, float sideLength) : this(pos, Vector2.one * sideLength)
        {
        }
        public Rect(Vector2 pos, float sideLength, Vector2 anchor) : this(pos, Vector2.one * sideLength, anchor)
        {
        }
        public Rect(Vector2 pos, Vector2 size)
        {
            this.pos = pos;
            this.size = size;
            pivot = Vectors.half2d;
        }
        public Rect(Vector2 pos, Vector2 size, Vector2 pivot) : this(pos, size)
        {
            this.pivot = pivot;
        }
        public void MovePivot(Vector2 pivot)
        {
            pos += size * pivot;
        }
        public void SetPivot(Vector2 pivot)
        {
            var os = pivot - this.pivot;
            pos += size * os;
            this.pivot = pivot;
        }
        public Vector2 cornerLB { get { return pos - size * Vectors.half2d; } }
        public Vector2 cornerLT { get { return pos - size * Vectors.half2d.ReverseY(); } }
        public Vector2 cornerRT { get { return pos + size * Vectors.half2d; } }
        public Vector2 cornerRB { get { return pos + size * Vectors.half2d.ReverseY(); } }
        /// <summary>
        /// LT
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Contains(Vector2 v)
        {
            var lt = pos - size * pivot;
            var rb = pos + size * (Vector2.one - pivot);
            return MathTool.Between(v, lt, rb);
        }
        /// <summary>
        /// ClockWise
        /// </summary>
        /// <returns></returns>
        public List<Vector2> ToPointsCW_()
        {
            var vs = new List<Vector2>();
            vs.Add(pos);
            vs.Add(pos + size * Vector2.right);
            vs.Add(pos + size);
            vs.Add(pos + size * Vector2.up);
            //vs.Add(pos + size * pivot.FlipRev());
            //vs.Add(pos + size * pivot.FlipRevY());
            //vs.Add(pos + size * pivot);
            //vs.Add(pos + size * pivot.FlipRevX());
            return vs;
        }
        public List<Vector2> ToPointsCW()
        {
            var vs = new List<Vector2>();
            vs.Add(pos);
            vs.Add(pos + size * Vector2.right);
            vs.Add(pos + size);
            vs.Add(pos + size * Vector2.up);

            for (int i = 0; i < vs.Count; i++)
            {
                vs[i] += size.X() * -pivot.x;
                vs[i] += size.Y() * pivot.y;
            }
            return vs;
        }
        public List<Vector2> ToCW()
        {
            var vs = new List<Vector2>();
            vs.Add(pos);
            vs.Add(pos + size * Vector2.right);
            vs.Add(pos + size);
            vs.Add(pos + size * Vector2.up);

            for (int i = 0; i < vs.Count; i++)
            {
                vs[i] += size * -pivot;
            }
            return vs;
        }
        public List<Vector2> ToPointsCWLT(int os = 0)
        {
            var vs = new List<Vector2>();
            vs.Add(pos);
            vs.Add(pos + size * Vector2.right);
            vs.Add(pos + size.ReverseY());
            vs.Add(pos + size * Vector2.down);

            for (int i = 0; i < vs.Count; i++)
            {
                vs[i] += size.X() * -pivot.x;
                vs[i] += size.Y() * (1 - pivot.y);
            }
            var vsos = new List<Vector2>();
            if (os != 0)
            {
                for (int i = 0; i < vs.Count; i++)
                {
                    var n = i + os;
                    while (n >= vs.Count) n -= vs.Count;
                    while (n < 0) n += vs.Count;
                    vsos.Add(vs[n]);
                }
                return vsos;
            }
            return vs;
        }
        public override string ToString()
        {
            return "pos: " + pos + ", " + "size: " + size + ".";
        }

        static bool Approx(float a, float b)
        {
            return Mathf.Approximately(a, b);
        }
        static bool Approx(Vector2 a, Vector2 b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);
        }
        static Vector2 ReverseY(Vector2 a)
        {
            return a * new Vector2(1, -1);
        }
    }
}