using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    [ExecuteInEditMode]
    public class GLDraw : MonoBehaviour
    {
        //public Vector3 gridSize = new Vector3(10, 0, 10);
        //public float smallStep = 0.5f;
        //public Color color = Color.white;
        //public Vector2 pos;
        //public Vector2 size;
        public Vector2[] v;
        public Color[] c;
        public Vector3[] direct;
        public Vector3[] vws;
        public Vector3[] vwsArrange;
        public int[] idxs = new int[] { 1, 0, 3, 2 };
        public float zOffset;
        public Vector3 vp;
        public Material mat;
        void OnRenderObject()
        {
            var cam = Camera.main;
            vp = cam.WorldToViewportPoint(transform.position);

            //GL.LoadOrtho();
            //GLUI._DrawLineDirect(v[0], v[1], Color.red, Color.black);
            mat.SetPass(0);
            GLUI._DrawQuadDirect_CusMat(c, direct);

            //vws = new Vector3[v.Length];
            //for (int j = 0; j < vws.Length; j++)
            //{
            //    vws[j] = cam.ViewportToWorldPoint(v[j].SetZ(zOffset));
            //}
            //vwsArrange = vws.Copy().Arrange(idxs);
            //GLUI._DrawQuadDirect(Color.red, direct);
        }
    }
}