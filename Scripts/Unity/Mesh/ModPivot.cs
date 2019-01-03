using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class ModPivot : MonoBehaviour
    {
        public Mesh originMesh;
        public Mesh modMesh;
        public Vector3 pivot;
        public Vector3 os = new Vector3(0, 0.5f, 0);
        private void Reset()
        {
            originMesh = GetComponent<MeshFilter>().sharedMesh;
            pivot = originMesh.GetCenter();
        }
        [Button]
        void Do()
        {
            var vs = new List<Vector3>();
            foreach (var v in originMesh.vertices)
            {
                vs.Add(v + os);
            }
            modMesh = new Mesh();
            modMesh.name = "Moded Pivot: " + vs.GetCenter().y;
            modMesh.vertices = vs.ToArray();
            modMesh.triangles = originMesh.triangles;
            modMesh.uv = originMesh.uv;
            modMesh.RecalculateNormals();
            modMesh.RecalculateBounds();
            GetComponent<MeshFilter>().sharedMesh = modMesh;
        }
#if UNITY_EDITOR
        [Button]
        void CreateAsset()
        {
            UnityEditor.AssetDatabase.CreateAsset(modMesh, @"Assets\" + originMesh.name + " mod.mesh");
        }
#endif
    }
}