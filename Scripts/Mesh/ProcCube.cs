using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa._UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Esa
{
    public class ProcCube : MonoBehaviour
    {
        public bool recalculateNormals = true;
        public bool recalculateTangents = true;
        public Material mat;
        public float sideLength = 1f;
        [Header("Independently Vertices")]
        public bool indep = true;
        [Button]
        void Cube()
        {
            var mf = this.GetComOrAdd<MeshFilter>();
            var mr = this.GetComOrAdd<MeshRenderer>();
            mf.sharedMesh = CubeMesh();
            mr.sharedMaterial = mat;
        }
        public int[] uvIdxs;
#if UNITY_EDITOR
        [Button]
        void SaveAsset()
        {
            var mesh = this.GetMesh();
            var path = @"Assets\" + mesh.name + ".mesh";
            AssetDatabase.CreateAsset(mesh, path);
        }
#endif
        Mesh CubeMesh()
        {
            var halfSide = sideLength * 0.5f;
            var ta = new Vector3(-halfSide, sideLength, -halfSide);
            var tb = new Vector3(-halfSide, sideLength, halfSide);
            var tc = new Vector3(halfSide, sideLength, halfSide);
            var td = new Vector3(halfSide, sideLength, -halfSide);

            var ba = ta + Vector3.down * sideLength;
            var bb = tb + Vector3.down * sideLength;
            var bc = tc + Vector3.down * sideLength;
            var bd = td + Vector3.down * sideLength;

            var vsShared = new List<Vector3>(new Vector3[] { ta, tb, tc, td, ba, bb, bc, bd });
            var quads = new List<Quad>();
            var uvSize = new Vector2(1f, 0.3333333f);
            var uvTop = UITool.GetVS(Vector2.up, uvSize, Vector2.up);
            var uvMid = UITool.GetVS(Vector2.up * 0.3333333f, uvSize, Vector2.zero);
            var uvBtm = UITool.GetVS(Vector2.zero, uvSize, Vector2.zero);
            // up
            quads.Add(new Quad(0, 1, 2, 3, uvTop));
            // forward
            quads.Add(new Quad(6, 2, 1, 5, uvMid));
            // back
            quads.Add(new Quad(4, 0, 3, 7, uvMid));
            // left
            quads.Add(new Quad(5, 1, 0, 4, uvMid));
            // right
            quads.Add(new Quad(7, 3, 2, 6, uvMid));
            // down
            quads.Add(new Quad(7, 6, 5, 4, uvBtm));

            //quads.Add(new Quad(0, 1, 2, 3));//, Vector3.up)); // up
            //quads.Add(new Quad(0, 1, 4, 5, true));//, Vector3.forward, true)); // forward
            //quads.Add(new Quad(2, 3, 6, 7));//, Vector3.back)); // back
            //quads.Add(new Quad(0, 2, 4, 6));//, Vector3.left)); // left
            //quads.Add(new Quad(1, 3, 5, 7, true));//, Vector3.right, true)); // right
            //quads.Add(new Quad(4, 5, 6, 7, true));//, Vector3.down, true)); // down

            var mesh = new Mesh();
            mesh.name = "Proc Cube";
            ApplyMesh(mesh, quads, vsShared, null, null, indep);
            return mesh;
        }

        void ApplyMesh(Mesh mesh, IList<Quad> quads, IList<Vector3> vertsShared,
               IList<Vector2> uvs, IList<Vector3> normsShared = null, bool indep = false)
        {
            var verts = indep ? Quad.ToVertsIndep(quads, vertsShared) : ListTool.IListToArray(vertsShared);
            var indices = Quad.ToIndices(quads, indep);

            mesh.vertices = verts;
            mesh.uv = Quad.ToUVsIndep(quads);
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);
            if (normsShared != null)
            {
                mesh.normals = Quad.ToNormals(quads, normsShared, indep);
            }
            if (recalculateNormals) mesh.RecalculateNormals();
            if (recalculateTangents) mesh.RecalculateTangents();
        }
    }
}