using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    [ExecuteInEditMode]
    public class TangentInspector : MonoBehaviour
    {
        public Mesh mesh;
        Vector3[] ts;
        Vector3[] vs;
        Vector3[] ns;
        Vector3[] bs;
        public float length = 0.1f;
        public bool depthTest = true;
        public bool recalculate = true;
        public int vertexCount;

        public bool drawNormal;
        public bool drawTangent;
        public bool drawBiTangent;
        [Button]
        public void RecalculateTangents()
        {
            mesh.RecalculateTangents();
            DoGetMesh();
        }
        [Button]
        public void RecalculateNormals()
        {
            mesh.RecalculateNormals();
            DoGetMesh();
        }
        [Button]
        public void DoGetMesh()
        {
            mesh = this.GetMesh();
            if (mesh == null) return;

            var tans = mesh.tangents;
            vs = mesh.vertices;
            ns = mesh.normals;
            ts = new Vector3[vs.Length];
            bs = new Vector3[vs.Length];
            for (int i = 0; i < vs.Length; i++)
            {
                vs[i] = transform.TransformPoint(vs[i]);
                ns[i] = transform.TransformDirection(ns[i]);
                ts[i] = transform.TransformDirection((Vector3)tans[i]);
                bs[i] = Vector3.Cross(ns[i], tans[i]) * tans[i].w;
            }
        }
        public bool onlyInspectIdxs;
        public List<int> inspectIdxs;
        void Update()
        {
            if (vs.Empty()) return;
            int i = 0;
            vertexCount = vs.Length;
            foreach (var v in vs)
            {
                if (!onlyInspectIdxs || inspectIdxs.Contains(i))
                {
                    if (drawNormal) Debug.DrawRay(v, ns[i] * length, Color.blue, 0, depthTest);
                    if (drawTangent) Debug.DrawRay(v, ts[i] * length, Color.red, 0, depthTest);
                    if (drawBiTangent) Debug.DrawRay(v, bs[i] * length, Color.green, 0, depthTest);
                }
                i++;
            }
        }
    }
}