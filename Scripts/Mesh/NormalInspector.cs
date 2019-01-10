using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class NormalInspector : MonoBehaviour
    {
        public Mesh mesh;
        public float length = 0.1f;
        Vector3[] ts;
        Vector3[] vs;
        Vector3[] ns;
        Vector3[] bs;
        public bool depthTest = true;
        public bool recalculate = true;
        public int vertexCount;
        [Button]
        void Start()
        {
            mesh = this.GetMesh();
            if (mesh == null) return;
            if (recalculate) mesh.RecalculateTangents();

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
        void Update()
        {
            if (vs.Empty()) return;
            int i = 0;
            vertexCount = vs.Length;
            foreach (var v in vs)
            {                
                Debug.DrawRay(v, ns[i] * length, Color.blue, 0, depthTest);
                Debug.DrawRay(v, ts[i] * length, Color.red, 0, depthTest);
                Debug.DrawRay(v, bs[i++] * length, Color.green, 0, depthTest);
            }
        }
    }
}