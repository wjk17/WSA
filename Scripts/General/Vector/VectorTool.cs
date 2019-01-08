using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Esa
{
    public static class VectorTool
    {
        public static Vector2Int Mul(this Vector2Int v, float f)
        {
            return new Vector2Int(Mathf.RoundToInt(v.x * f), Mathf.RoundToInt(v.y * f));
        }
        public static int eleCount(this Vector2Int v)
        {
            return v.x * v.y;
        }
        public static Vector3Int RoundToInt(this Vector3 f)
        {
            return new Vector3Int(Mathf.RoundToInt(f.x), Mathf.RoundToInt(f.y), Mathf.RoundToInt(f.z));
        }
        public static Vector2Int RoundToInt(this Vector2 f)
        {
            return new Vector2Int(Mathf.RoundToInt(f.x), Mathf.RoundToInt(f.y));
        }
        public static Vector2Int Clamp(this Vector2Int v, Vector2Int min, Vector2Int max)
        {
            v.x = Mathf.Clamp(v.x, min.x, max.x);
            v.y = Mathf.Clamp(v.y, min.y, max.y);
            return v;
        }
        public static Vector2Int ToInt(this Vector2 v)
        {
            return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
        }
        public static Vector3 Lerp(this Vector3[] vs, float t)
        {
            return Vector3.Lerp(vs[0], vs[1], t);
        }
        public static Vector3[] Mul3x4(this Vector3[] vs, Matrix4x4 m)
        {
            var list = new List<Vector3>();
            foreach (var v in vs)
            {
                list.Add(m.MultiplyPoint3x4(v));
            }
            return list.ToArray();
        }
        public static float Repeat(float t, float length)
        {
            if (length == 0) return 0f;
            if (t / length == 1f) return length;
            return (t - (Mathf.Floor(t / length) * length));
        }
        public static float MapRange(this float value, float min1, float max1, float min2, float max2)
        {
            return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
        }
        public static bool IsEven(this int n)
        {
            return !Convert.ToBoolean(n & 1);
        }
        public static bool IsOdd(this int n)
        {
            return Convert.ToBoolean(n & 1);
        }
        public static Vector3 ColumnsV3(int left, int top, int columnWeight, int bottom, int value)
        {
            return new Vector3(left + value / bottom * columnWeight, top + (value % bottom), 0f);
        }
        public static Vector3 ColumnsV3(float left, float top, float columnWeight, float bottom, float value)
        {
            return new Vector3(left + (value / bottom) * columnWeight, top + (value % bottom), 0f);
        }
        public static Vector2 Columns(float left, float top, float columnWeight, float bottom, float value)
        {
            return new Vector2(left + (value / bottom) * columnWeight, top + (value % bottom));
        }
        // 获取选中对象Pivot
        public static Vector3 GetPivot(Transform[] trans)
        {
            if (trans == null || trans.Length == 0)
                return Vector3.zero;
            if (trans.Length == 1)
                return trans[0].position;

            float minX = Mathf.Infinity;//无限大
            float minY = Mathf.Infinity;
            float minZ = Mathf.Infinity;

            float maxX = -Mathf.Infinity;//无限小
            float maxY = -Mathf.Infinity;
            float maxZ = -Mathf.Infinity;

            foreach (Transform tr in trans)//AABB
            {
                if (tr.position.x < minX)
                    minX = tr.position.x;
                if (tr.position.y < minY)
                    minY = tr.position.y;
                if (tr.position.z < minZ)
                    minZ = tr.position.z;

                if (tr.position.x > maxX)
                    maxX = tr.position.x;
                if (tr.position.y > maxY)
                    maxY = tr.position.y;
                if (tr.position.z > maxZ)
                    maxZ = tr.position.z;
            }

            return new Vector3((minX + maxX) / 2.0f, (minY + maxY) / 2.0f, (minZ + maxZ) / 2.0f);
        }
        // 计算中心点
        public static Vector3 GetCenter(this Mesh mesh)
        {
            var pos = Vector3.zero;
            foreach (var v in mesh.vertices)
            {
                pos += v;
            }
            pos /= (mesh.vertices.Length - 1);
            return pos;
        }

        public static Vector3 GetCenter(this IList<Vector3> vs)
        {
            var pos = Vector3.zero;
            foreach (var v in vs)
            {
                pos += v;
            }
            pos /= (vs.Count - 1);
            return pos;
        }
        public static Vector3 GetCenter(GameObject[] g)
        {
            Vector3 center = Vector3.zero;
            for (int i = 0; i < g.Length; i++)
            {
                center += g[i].transform.position;
            }
            var selectedCount = g.Length;
            center = new Vector3(center.x / selectedCount, center.y / selectedCount, center.z / selectedCount);
            return center;
        }
        public static Vector3 Center(this Transform transform)
        {
            Vector3 center = Vector3.zero;
            bool first = true;
            foreach (Transform t in transform)
            {
                if (first)
                {
                    first = false;
                    center = t.position;
                }
                else
                {
                    Vector3 v3 = t.position - center;
                    center += v3.normalized * v3.magnitude * 0.5f;
                }
            }
            return center;
        }
        public static Vector3 signY(this Vector3 V, float sign)
        {
            Vector3 result;
            if (Mathf.Sign(V.y) != sign) result = new Vector3(V.x, V.y * -1, V.z);
            else result = V;
            return result;
        }
        public static Vector2 Lerp2(this Vector2 a, Vector2 b, Vector2 t)
        {
            var n = new Vector2(
                    Mathf.Lerp(a.x, b.x, t.x),
                    Mathf.Lerp(a.y, b.y, t.y));
            return n;
        }
        public static Vector2 Divide(this Vector2 a, Vector2 b)
        {
            Vector2 n;
            n.x = a.x / b.x;
            n.y = a.y / b.y;
            return n;
        }
        public static void Divide(ref Vector2 a, Vector2 b)
        {
            a.x = a.x / b.x;
            a.y = a.y / b.y;
        }
        public static Vector3 Divide(this Vector3 a, Vector3 b)
        {
            a.x = a.x / b.x;
            a.y = a.y / b.y;
            a.z = a.z / b.z;
            return a;
        }
        public static void Divide(ref Vector3 a, Vector3 b)
        {
            a.x = a.x / b.x;
            a.y = a.y / b.y;
            a.z = a.z / b.z;
        }
        public static Vector2Int DivideToInt(this Vector2 a, float b)
        {
            var i = new Vector2Int();
            i.x = Mathf.RoundToInt(a.x / b);
            i.y = Mathf.RoundToInt(a.y / b);
            return i;
        }
        public static Vector3Int DivideToInt(this Vector3 a, float b)
        {
            var i = new Vector3Int();
            i.x = Mathf.RoundToInt(a.x / b);
            i.y = Mathf.RoundToInt(a.y / b);
            i.z = Mathf.RoundToInt(a.z / b);
            return i;
        }
        public static Vector3Int DivideToInt(this Vector3 a, Vector3 b)
        {
            var i = new Vector3Int();
            i.x = Mathf.RoundToInt(a.x / b.x);
            i.y = Mathf.RoundToInt(a.y / b.y);
            i.z = Mathf.RoundToInt(a.z / b.z);
            return i;
        }
        public static Vector2 Snap(this Vector2 a, float b)
        {
            return (Vector2)a.DivideToInt(b) * b;
        }
        public static Vector3 Snap(this Vector3 a, float b)
        {
            return (Vector3)a.DivideToInt(b) * b;
        }
        /// <summary>
        /// 曼哈顿距离
        /// </summary>
        public static int Manhattan(this Vector2Int a, int x, int y)
        {
            return Mathf.Abs(a.y - y) + Mathf.Abs(a.x - x);
        }
        public static int Manhattan(this Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.y - b.y) + Mathf.Abs(a.x - b.x);
        }
        public static float Manhattan(this Vector2 a, float x, float y)
        {
            return Mathf.Abs(a.y - y) + Mathf.Abs(a.x - x);
        }
        public static float Manhattan(this Vector2 a, Vector2 b)
        {
            return Mathf.Abs(a.y - b.y) + Mathf.Abs(a.x - b.x);
        }
        public static Vector2 Abs(this Vector2 a)
        {
            return new Vector2(Mathf.Abs(a.x), Mathf.Abs(a.y));
        }
        public static Vector3 Abs(this Vector3 a)
        {
            return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Vector2 SetX(this Vector2 vec, float f)
        {
            return new Vector2(f, vec.y);
        }
        public static Vector2 SetY(this Vector2 vec, float f)
        {
            return new Vector2(vec.x, f);
        }
        public static Vector3 SetXZ(this Vector3 vec, Vector3 f)
        {
            return new Vector3(f.x, vec.y, f.z);
        }
        public static Vector3 SetXZ(this Vector3 vec, Vector2 f)
        {
            return new Vector3(f.x, vec.y, f.y);
        }
        public static Vector3 SetXY(this Vector3 vec, Vector2 f)
        {
            return new Vector3(f.x, f.y, vec.z);
        }
        public static Vector3 SetX(this Vector3 vec, float f)
        {
            return new Vector3(f, vec.y, vec.z);
        }
        public static Vector3 SetY(this Vector3 vec, float f)
        {
            return new Vector3(vec.x, f, vec.z);
        }
        public static Vector3 SetZ(this Vector3 vec, float f)
        {
            return new Vector3(vec.x, vec.y, f);
        }
        /// v2 to v3
        public static Vector3 X0Y(this Vector2 v2)
        {
            return new Vector3(v2.x, 0f, v2.y);

        }
        public static Vector3 XY0(this Vector2 v2)
        {
            return new Vector3(v2.x, v2.y);

        }
        /// v3 to v2
        public static Vector2 XY(this Vector3 v3)
        {
            return new Vector2(v3.x, v3.y);
        }
        public static Vector2 XZ(this Vector3 v3)
        {
            return new Vector2(v3.x, v3.z);
        }
        /// v3
        public static Vector3 toX00(this Vector3 v3)
        {
            return new Vector3(v3.x, 0f);
        }
        public static Vector3 to1Y1(this Vector3 v3)
        {
            return new Vector3(1f, v3.y, 1f);
        }
        public static Vector3 toX1Z(this Vector3 v3)
        {
            return new Vector3(v3.x, 1f, v3.z);
        }
        public static Vector3 to0Y0(this Vector3 v3)
        {
            return new Vector3(0f, v3.y);
        }
        public static Vector3 to00Z(this Vector3 v3)
        {
            return new Vector3(0f, 0f, v3.z);
        }
        public static Vector3 XY0(this Vector3 v3)
        {
            return new Vector3(v3.x, v3.y);
        }
        public static Vector3 toXY1(this Vector3 v3)
        {
            return new Vector3(v3.x, v3.y, 1f);
        }
        public static Vector3 toX0Z(this Vector3 v3)
        {
            return new Vector3(v3.x, 0f, v3.z);
        }
        public static Vector3 to0YZ(this Vector3 v3)
        {
            return new Vector3(0, v3.y, v3.z);
        }
        public static Vector2 yzV2(this Vector3 v3)
        {
            return new Vector2(v3.y, v3.z);
        }
        public static Vector3 toYZ(this Vector2 v2)
        {
            return new Vector3(0, v2.x, v2.y);
        }
        public static Vector2Int Y(this Vector2Int v2)
        {
            return new Vector2Int(0, v2.y);
        }
        public static Vector2 Y(this Vector2 v2)
        {
            return new Vector2(0, v2.y);
        }
        public static Vector2 To0Y(this Vector2 v2)
        {
            return new Vector2(0, v2.y);
        }
        public static Vector2Int X(this Vector2Int v2)
        {
            return new Vector2Int(v2.x, 0);
        }
        public static Vector2 X(this Vector2 v2)
        {
            return new Vector2(v2.x, 0);
        }
        public static Vector2 ToX0(this Vector2 v2)
        {
            return new Vector2(v2.x, 0);
        }

        internal static Vector2Int Clamp(object v)
        {
            throw new NotImplementedException();
        }
    }
}