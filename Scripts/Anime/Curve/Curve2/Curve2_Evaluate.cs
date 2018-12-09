using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public partial class Curve2
    {
        Vector2 EvaluateAOut(Key2 a, Key2 b, float t)
        {
            var points = new Vector2[4];
            points[0] = a.vector;
            points[1] = a.outTan;
            points[2] = b.vector;
            var Points2 = new Vector2[2];
            for (int i = 0; i < 2; i++)
            {
                Points2[i] = Vector2.Lerp(points[i], points[i + 1], t);
            }
            return Vector2.Lerp(Points2[0], Points2[1], t);
        }
        Vector2 EvaluateBIn(Key2 a, Key2 b, float t)
        {
            var points = new Vector2[4];
            points[0] = a.vector;
            points[1] = b.inTan;
            points[2] = b.vector;
            var Points2 = new Vector2[2];
            for (int i = 0; i < 2; i++)
            {
                Points2[i] = Vector2.Lerp(points[i], points[i + 1], t);
            }
            return Vector2.Lerp(Points2[0], Points2[1], t);
        }
        /// <summary>
        /// 注意顺序：a在前b在后，使用a的outT，b的InT
        /// </summary>
        public static Vector2 Evaluate(Key2 a, Key2 b, float t)
        {
            var list = new List<Vector2>();
            list.Add(a.vector);
            if (a.outMode == KeyMode.Bezier) list.Add(a.outTan);
            if (b.inMode == KeyMode.Bezier) list.Add(b.inTan);
            list.Add(b.vector);
            return Evaluate1to4(list, t);
        }
        public static Vector2 Evaluate1to4(IList<Vector2> ts, float t)
        {
            if (ts.Count == 4) return Evaluate4(ts, t);
            if (ts.Count == 3) return Evaluate3(ts, t);
            if (ts.Count == 2) return Vector2.Lerp(ts[0], ts[1], t);
            if (ts.Count == 1) return ts[0];
            throw new System.Exception();
        }
        public static Vector2 Evaluate4(IList<Vector2> ts, float t)
        {
            var points = new Vector2[4];
            points[0] = ts[0];
            points[1] = ts[1];
            points[2] = ts[2];
            points[3] = ts[3];
            var points3 = new Vector2[3];
            for (int i = 0; i < 3; i++)
            {
                points3[i] = Vector2.Lerp(points[i], points[i + 1], t);
            }
            var points2 = new Vector2[2];
            for (int i = 0; i < 2; i++)
            {
                points2[i] = Vector2.Lerp(points3[i], points3[i + 1], t);
            }
            return Vector2.Lerp(points2[0], points2[1], t);
        }
        public static Vector2 Evaluate3(IList<Vector2> ts, float t)
        {
            var points = new Vector2[3];
            points[0] = ts[0];
            points[1] = ts[1];
            points[2] = ts[2];
            var points2 = new Vector2[2];
            for (int i = 0; i < 2; i++)
            {
                points2[i] = Vector2.Lerp(points[i], points[i + 1], t);
            }
            return Vector2.Lerp(points2[0], points2[1], t);
        }
        public Vector2 Evaluate2D(float t)
        {
            if (Count == 0) return new Vector2(t, 0f);
            var i = 1;
            for (; i < Count; i++)
            {
                if (t < keys[i].time)
                {
                    goto findIndex;
                }
            }
            return keys[Count - 1].vector;
            findIndex:
            i--;
            var tLocal = t - keys[i].time;
            var tN = tLocal / (keys[i + 1].time - keys[i].time);
            return Evaluate(this[i], this[i + 1], tN);
        }
        public float Evaluate1D(float t)
        {
            return Evaluate2D(t).y;
        }
    }
}