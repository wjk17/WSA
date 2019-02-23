using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using UI_;
    public class MeshEdit : MonoBehaviour
    {
        public List<Vector2> vs;
        public Vector2 size;
        public Vector2 pivot = Vectors.half2d;
        void Start()
        {
            this.AddInput(Input, 0);
        }
        void Input()
        {
            this.BeginOrtho();
            foreach (var v in vs)
            {
                var verts = UITool.GetVS(v, size, pivot);
                GLUI.DrawQuad(verts, Color.black);
            }
        }
    }
}