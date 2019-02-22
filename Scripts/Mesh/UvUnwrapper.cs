using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class UvUnwrapper : MonoBehaviour
    {
        public Mesh mesh;
        public Vector2[] uv;
        public Vector3[] vs;
        public int vsCount;
        [Button]
        void Apply()
        {
            mesh.uv = uv;
        }
        private void Update()
        {

        }
        [Button]
        void GetInfo()
        {
            mesh = this.GetMesh();
            uv = mesh.uv;
            vs = mesh.vertices;
            vsCount = mesh.vertexCount;
        }
    }
}