using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa
{
    using UI_;
    [Serializable]
    public class MeshQuad
    {
        public Mesh _mesh;
        public List<Vector3> vertex = new List<Vector3>();
        public List<int> triangle = new List<int>();
        public List<Vector3> normal = new List<Vector3>();
        public List<Vector2> uv = new List<Vector2>();
        public List<Vector4> tangent = new List<Vector4>();
        //public MeshQuad(Mesh mesh)
        //{
        //    vertex = new List<Vector3>(mesh.vertices);
        //    triangle = new List<int>(mesh.triangles);
        //    normal = new List<Vector3>(mesh.normals);
        //    uv = new List<Vector2>(mesh.uv);
        //    tangent = new List<Vector4>(mesh.tangents);
        //}
        public List<Quad> quads = new List<Quad>();
        public List<Vector3> vertexShared;
        public static float combineDist = 0.0001f;

        public void Apply()
        {
            _mesh.SetVertices(vertex);
            _mesh.SetTriangles(triangle, 0);
            _mesh.SetNormals(normal);
            _mesh.SetUVs(0, uv);
            _mesh.SetTangents(tangent);
        }
        public MeshQuad(Mesh mesh)
        {
            _mesh = mesh;
            mesh.GetVertices(vertex);
            mesh.GetTriangles(triangle, 0);
            mesh.GetNormals(normal);
            mesh.GetUVs(0, uv);
            mesh.GetTangents(tangent);
            GetShared();
        }
        public void ApplyShared()
        {
            _mesh.vertices = Quad.ToVertsIndep(quads, vertexShared);
            _mesh.triangles = Quad.ToIndices(quads, true);
        }
        private void GetShared()
        {
            vertexShared = ToSharedVS(vertex);
            //normalShared = ToSharedVS(normal);
            quads = ToSharedIdx(triangle);
            MakeUnique();
        }
        private void MakeUnique()
        {
            var vertexUnique = new List<Vector3>();
            var newQuads = new List<Quad>();
            foreach (var q in quads)
            {
                var vs = q.Vertex(vertexShared);
                var index = new List<int>();
                foreach (var v in vs)
                {
                    var idx = vertexUnique.IndexOfApprox(v, combineDist);
                    if (idx > -1)
                    {
                        index.Add(idx);
                    }
                    else
                    {
                        vertexUnique.Add(v);
                        index.Add(vertexUnique.Count - 1);
                    }
                }
                newQuads.Add(new Quad(index, q.UV));
            }
            vertexShared = vertexUnique;
            quads = newQuads;
        }

        private List<Quad> ToSharedIdx(List<int> tri)
        {
            var list = new List<Quad>();
            for (int i = 0; i < tri.Count; i += 6)
            {
                //list.Add(new Quad(tri[i], tri[i + 1], tri[i + 2], tri[i + 3]));
                var idx = i / 6 * 4;
                list.Add(new Quad(idx, idx + 1, idx + 2, idx + 3));
            }
            return list;
        }

        public List<Vector3> ToSharedVS(List<Vector3> vs)
        {
            var list = new List<Vector3>();
            for (int i = 0; i < vs.Count; i += 6)
            {
                list.Add(vs[i]);
                list.Add(vs[i + 1]);
                list.Add(vs[i + 2]);
                list.Add(vs[i + 3]);
            }
            return list;
        }
    }
    public class MeshEdit2D : MonoBehaviour
    {
        public float zOffset = -0.01f;
        public Vector2 size;
        public List<Vector2> vs;
        public MeshQuad mq;
        Camera cam;
        public int idx = -1;
        public List<int> idxs;
        void Start()
        {
            this.AddInput(Input, -200);
            mq = new MeshQuad(this.GetMesh());
            cam = Camera.main;
        }
        void Input()
        {
            this.StartGLWorld(200);
            var v3 = new List<Vector3>();
            foreach (var v in mq.vertexShared)
            {
                v3.Add(transform.TransformPoint(v));
            }
            foreach (var q in mq.quads)
            {
                var ps = q.index;
                var colors = new List<Color>();
                foreach (var p in ps)
                {
                    var sel = idxs.Contains(p);
                    colors.Add(sel ? Color.yellow : Color.black);
                }
                GLUI.DrawLineDirect(v3[q.a], v3[q.b], colors[0], colors[1]);
                GLUI.DrawLineDirect(v3[q.b], v3[q.c], colors[1], colors[2]);
                GLUI.DrawLineDirect(v3[q.c], v3[q.d], colors[2], colors[3]);
                GLUI.DrawLineDirect(v3[q.d], v3[q.a], colors[3], colors[0]);
            }
            var vRectSize = (size * 10).ToNDC();
            var vSize = size.ToNDC();
            GLUI.BeginOrder(1);
            if (Events.KeyDown(KeyCode.A))
            {
                GizmosAxis.I.Disactive();
                idx = -1;
            }
            int i = 0;
            bool click = false;
            foreach (var v in mq.vertexShared)
            {
                var vp = cam.WorldToViewportPoint(v3[i]);
                // 点击
                if (!click && Events.MouseDown1)
                {
                    var rect = new Rect(vp, vRectSize, Vectors.half2d);
                    if (rect.Contains(UI.mousePosView))
                    {
                        click = true;
                        idxs.Clear();
                        idxs.Add(i);
                        idx = i;
                        GizmosAxis.I.Active();
                        GizmosAxis.I.transform.position = v3[i];
                    }
                }
                // 绘制
                var vps = UITool.GetVS(vp, vSize, Vectors.half2d);
                var vps3 = vps.SetZ(vp.z + zOffset);
                for (int j = 0; j < vps.Length; j++)
                {
                    vps3[j] = cam.ViewportToWorldPoint(vps3[j]);
                }

                GLUI.DrawQuadDirect(i == idx ? Color.yellow : Color.black, vps3);
                i++;
            }
            if (idx > -1) mq.vertexShared[idx] = GizmosAxis.I.transform.position;
            //mq.Apply();
            mq.ApplyShared();
        }
    }
}