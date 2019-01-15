using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
namespace Esa
{
    /// <summary>
    /// 通过顶点顺序让Unity自动生成法线
    /// </summary>
    [Serializable]
    public class Quad : ICloneable
    {
        public int a, b, c, d;
        public Vector2 uvA, uvB, uvC, uvD;
        public Vector3[] Vertex(IList<Vector3> vertex) { return new Vector3[] { vertex[a], vertex[b], vertex[c], vertex[d] }; }
        public int[] Index
        {
            get { return new int[] { a, b, c, d }; }
            set { a = value[0]; b = value[1]; c = value[2]; d = value[3]; }
        }
        public Vector2[] UV
        {
            get { return new Vector2[] { uvA, uvB, uvC, uvD }; }
        }
        public Quad(IList<int> v, IList<Vector2> uv)
        {
            a = v[0]; b = v[1]; c = v[2]; d = v[3];
            uvA = uv[0]; uvB = uv[1]; uvC = uv[2]; uvD = uv[3];
        }
        public Quad(params int[] vs)
        {
            Index = vs;
        }
        public Quad(int a, int b, int c, int d)//, Vector3 normal)
        {
            this.a = a; this.b = b; this.c = c; this.d = d;
            //this.normal = normal;
            //na = nb = nc = nd = normal;
        }
        public Quad(int a, int b, int c, int d, params Vector2[] uv)
        {
            this.a = a; this.b = b; this.c = c; this.d = d;
            uvA = uv[0]; uvB = uv[1]; uvC = uv[2]; uvD = uv[3];
            //this.normal = normal;
            //na = nb = nc = nd = normal;
        }
        public Quad(int a, int b, int c, int d, bool reverseNormal)//Vector3 normal, bool reverseNormal)
        {
            if (reverseNormal)
            {
                this.a = d; this.b = b; this.c = c; this.d = a;
            }
            else
            {
                this.a = a; this.b = b; this.c = c; this.d = d;
            }
            //na = nb = nc = nd = normal;
        }
        public Quad(int a, int b, int c, int d, bool reverseNormal, params Vector2[] uv)
        {
            if (reverseNormal)
            {
                this.a = d; this.b = b; this.c = c; this.d = a;
                uvA = uv[3]; uvB = uv[1]; uvC = uv[2]; uvD = uv[0];
            }
            else
            {
                uvA = uv[0]; uvB = uv[1]; uvC = uv[2]; uvD = uv[3];
                this.a = a; this.b = b; this.c = c; this.d = d;
            }
            //na = nb = nc = nd = normal;
        }

        //public Vector3 normal;
        //public Vector3 na, nb, nc, nd;
        public static Quad ToReverse(Quad quad)
        {
            ComTool.Swap(ref quad.a, ref quad.d);
            return quad;
        }
        public static IList<Quad> ToReverse(IList<Quad> quads)
        {
            for (int i = 0; i < quads.Count; i++)
            {
                quads[i] = ToReverse(quads[i]);
            }
            return quads;
        }
        public static int[] ToIndices(Quad quad)
        {
            return new int[] { quad.a, quad.b, quad.c, quad.d, quad.c, quad.b };
        }
        public static int[] ToIndices(IList<Quad> quads, bool indep)
        {
            if (indep) return ToIndicesIndep(quads);
            else return ToIndicesShared(quads);
        }
        private static int[] ToIndicesShared(IList<Quad> quads)
        {
            var idxs = new int[quads.Count * 6];
            int i = 0;
            foreach (var quad in quads)
            {
                idxs[i] = quad.a;
                idxs[i + 1] = quad.b;
                idxs[i + 2] = quad.c;
                idxs[i + 3] = quad.d;
                idxs[i + 4] = quad.c;
                idxs[i + 5] = quad.b;
                i += 6;
            }
            return idxs;
        }
        private static int[] ToIndicesIndep(IList<Quad> quads)
        {
            var idxs = new int[quads.Count * 6];
            int i = 0;
            foreach (var quad in quads)
            {
                idxs[i] = i;//quad.a
                idxs[i + 1] = i + 1;//quad.b;
                idxs[i + 2] = i + 2;//quad.c;
                idxs[i + 3] = i + 3;//quad.d;
                idxs[i + 4] = i + 4;//quad.c;
                idxs[i + 5] = i + 5;//quad.b;
                i += 6;
            }
            return idxs;
        }
        public static Vector3[] ToVertsIndep(IList<Quad> quads, IList<Vector3> vertsShared)
        {
            var vs = new Vector3[quads.Count * 6];
            int i = 0;
            foreach (var quad in quads)
            {
                vs[i] = vertsShared[quad.a];
                vs[i + 1] = vertsShared[quad.b];
                vs[i + 2] = vertsShared[quad.c];
                vs[i + 3] = vertsShared[quad.d];
                vs[i + 4] = vertsShared[quad.a];
                vs[i + 5] = vertsShared[quad.c];
                i += 6;
            }
            return vs;
        }
        public static Vector2[] ToUVsIndep(IList<Quad> quads)
        {
            var uv = new Vector2[quads.Count * 6];
            int i = 0;
            foreach (var quad in quads)
            {
                uv[i] = quad.uvA;
                uv[i + 1] = quad.uvB;
                uv[i + 2] = quad.uvC;
                uv[i + 3] = quad.uvD;
                uv[i + 4] = quad.uvA;
                uv[i + 5] = quad.uvC;
                i += 6;
            }
            return uv;
        }
        internal static Vector3[] ToNormals(IList<Quad> quads, IList<Vector3> normals, bool indep)
        {
            if (indep) return ToNormalsIndep(quads, normals);
            else return ToNormalsShared(quads, normals);
        }
        private static Vector3[] ToNormalsShared(IList<Quad> quads, IList<Vector3> normalsShared)
        {
            var ns = new Vector3[quads.Count * 4];
            int i = 0;
            foreach (var quad in quads)
            {
                ns[i] = normalsShared[quad.a];
                ns[i + 1] = normalsShared[quad.b];
                ns[i + 2] = normalsShared[quad.c];
                ns[i + 3] = normalsShared[quad.d];
                i += 4;
            }
            return ns;
        }
        private static Vector3[] ToNormalsIndep(IList<Quad> quads, IList<Vector3> normalsIndep)
        {
            var ns = new Vector3[quads.Count * 6];
            int i = 0;
            foreach (var quad in quads)
            {
                ns[i] = normalsIndep[quad.a];
                ns[i + 1] = normalsIndep[quad.b];
                ns[i + 2] = normalsIndep[quad.c];
                ns[i + 3] = normalsIndep[quad.d];
                ns[i + 4] = normalsIndep[quad.a];
                ns[i + 5] = normalsIndep[quad.c];
                i += 6;
            }
            return ns;
        }
        public object Clone()
        {
            Quad q = new Quad(a, b, c, d);
            return q;
        }
        public static IList<Quad> Clone(IList<Quad> quads)
        {
            var list = new List<Quad>();
            foreach (var quad in quads)
            {
                list.Add(ToReverse((Quad)quad.Clone()));
            }
            return list;
        }

        internal static void Offset(IList<Quad> quads, int offset)
        {
            for (int i = 0; i < quads.Count; i++)
            {
                quads[i].a += offset;
                quads[i].b += offset;
                quads[i].c += offset;
                quads[i].d += offset;
            }
        }
    }
}