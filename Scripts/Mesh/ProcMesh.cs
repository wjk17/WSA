using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    [ExecuteInEditMode]
    public class ProcMesh : MonoBehaviour
    {
        public Vector3[] vs;
        public Vector3[] ns;
        public Vector2[] uv;
        public Vector4[] ts;
        public Material mat;
        public Mesh mesh;
        public bool modPos = false;
        public int idx;
        public Vector3 pos;
        public float radius;

        [Button]
        void ModVertex()
        {
            modPos = true;
            pos = mesh.vertices[idx];
        }
        [Button]
        void DoGetMesh() { mesh = this.GetMesh(); }
        void Update()
        {
            if (!modPos || mesh == null) return;
            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[idx] = pos;
            }
            mesh.vertices = vertices;
            //this.SetMesh(mesh);
        }
        private void OnDrawGizmos()
        {
            if (mesh != null)
            {
                pos = mesh.vertices[idx];
                Gizmos.DrawWireSphere(transform.TransformPoint(pos), radius);
            }
        }
        public bool revNormal;
        public bool revTangent;
        public int[] tris;
        [Button]
        void ApplyNormal()
        {
            ns[18] = ns[18].normalized;
            ns[19] = ns[19].normalized;
            ns[20] = ns[20].normalized;            
            ts[18] = ts[18].Normalize3();
            ts[19] = ts[19].Normalize3();
            ts[20] = ts[20].Normalize3();
            mesh.normals = ns;
            mesh.tangents = ts;
            GetComponent<TangentInspector>().DoGetMesh();
        }
        [Button]
        void RecalNormal()
        {
            tris = mesh.triangles;
            vs = mesh.vertices;
            ns = mesh.normals;
            ts = mesh.tangents;
            for (int i = 0; i < tris.Length; i += 3)
            {
                var v1 = vs[tris[i]];
                var v2 = vs[tris[i + 1]];
                var v3 = vs[tris[i + 2]];
                var t1 = v2 - v1;
                var t2 = v3 - v2;
                var normal = revNormal ? Vector3.Cross(t2, t1) : Vector3.Cross(t1, t2);
                Vector4 tangent = t1;
                tangent.w = revTangent ? -1 : 1;
                normal = normal.normalized;
                ns[tris[i]] = ns[tris[i + 1]] = ns[tris[i + 2]] = normal;
                ts[tris[i]] = ts[tris[i + 1]] = ts[tris[i + 2]] = tangent;
            }
            mesh.normals = ns;
            mesh.tangents = ts;
            //mesh.RecalculateNormals();
            //mesh.RecalculateTangents();
            GetComponent<TangentInspector>().DoGetMesh();
        }
        [Button]
        void Generate()
        {
            var mf = this.GetComOrAdd<MeshFilter>();
            var mr = this.GetComOrAdd<MeshRenderer>();
            mf.sharedMesh = NewMesh();
            mr.sharedMaterial = mat;
        }
        Mesh NewMesh()
        {
            var mesh = new Mesh();
            mesh.name = "ProcMesh";
            mesh.vertices = vs;
            mesh.triangles = new int[] { 0, 1, 2, 3, 2, 1 };
            //mesh.normals = ns;
            mesh.uv = uv;
            return mesh;
        }
    }
}