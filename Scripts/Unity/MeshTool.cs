using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public enum MeshType
    {
        Skinned,
        Normal,
    }
    public class MeshWrapper
    {
        public MeshType meshType;
        public Mesh mesh;
        public MeshFilter mf;
        public SkinnedMeshRenderer smr;
        public static implicit operator MeshWrapper(Transform t)
        {
            return new MeshWrapper(t);
        }
        public MeshWrapper(Transform t)
        {
            smr = t.GetComponent<SkinnedMeshRenderer>();
            if (smr != null)
            {
                smr.sharedMesh = Mesh.Instantiate(smr.sharedMesh);
                meshType = MeshType.Skinned;
                mesh = smr.sharedMesh;
            }
            else
            {
                mf = t.GetComponent<MeshFilter>();
                if (mf != null)
                {
                    meshType = MeshType.Normal;
                    mesh = mf.mesh;
                }
                else
                {
                    throw new System.Exception("no smr or mf");
                }
            }
        }
        public void BakeSkinMesh()
        {
            smr.BakeMesh(mesh);
            var go = smr.gameObject;
            var mats = smr.sharedMaterials;
            UnityEngine.Object.Destroy(smr);
            mf = go.AddComponent<MeshFilter>();
            mf.mesh = mesh;
            var mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterials = mats;
        }
    }
    public static class MeshTool
    {
        public static void SetMesh(this MonoBehaviour mono, Mesh mesh)
        {
            var smr = mono.GetComponent<SkinnedMeshRenderer>();
            if (smr != null) smr.sharedMesh = mesh;
            else
            {
                var mf = mono.GetComponent<MeshFilter>();
                if (mf != null) mf.sharedMesh = mesh;
            }
        }
        public static Mesh GetMesh(this MonoBehaviour mono)
        {
            Mesh mesh = null;
            var smr = mono.GetComponent<SkinnedMeshRenderer>();
            if (smr != null) mesh = smr.sharedMesh;
            else
            {
                var mf = mono.GetComponent<MeshFilter>();
                if (mf != null) mesh = mf.sharedMesh;
            }
            return mesh;
        }
        public static Material GetMat(this MonoBehaviour mono)
        {
            Material mat = null;
            var smr = mono.GetComponent<SkinnedMeshRenderer>();
            if (smr != null) mat = smr.sharedMaterial;
            else
            {
                var mr = mono.GetComponent<MeshRenderer>();
                if (mr != null) mat = mr.sharedMaterial;
            }
            return mat;
        }

        public static void UseBinormalAsTangent(this SkinnedMeshRenderer smr)
        {
            var mesh = Object.Instantiate(smr.sharedMesh);
            var ts = mesh.tangents;
            var ns = mesh.normals;
            for (int i = 0; i < ns.Length; i++)
            {
                var bi = Vector3.Cross(ns[i], ts[i]) * ts[i].w;
                ts[i] = new Vector4(bi.x, bi.y, bi.z, 1f);
            }
            mesh.tangents = ts;
            smr.sharedMesh = mesh;
        }

        public static Vector3[] GetVerts(this Mesh mesh, int[] idxs)
        {
            var vs = mesh.vertices;
            var list = new List<Vector3>();
            foreach (var idx in idxs)
            {
                list.Add(vs[idx]);
            }
            return list.ToArray();
        }
        public static string ToString(this Mesh mesh)
        {
            return mesh.vertexCount.ToString() + "vertices, " +
                mesh.boneWeights.Length.ToString() + " weights, ";
        }
        public static string ToString(this SkinnedMeshRenderer smr)
        {
            return smr.sharedMesh.ToString() +
                smr.bones.Length.ToString() + " bones";
        }
        public static void SetYToOne(this MeshFilter mf, bool copy = true)
        {
            SetYTo(mf, 1f, copy);
        }
        // must applyscale before this
        public static void SetYTo(this MeshFilter mf, float Y, bool copy = true)
        {
            float yMax = 0f;
            var m = mf.sharedMesh;
            if (copy) m = Object.Instantiate(m);
            var vs = m.vertices;
            foreach (var v in vs)
            {
                var y = v.y;
                yMax = Mathf.Max(yMax, y);
            }
            var ratio = Y / yMax;
            for (int i = 0; i < vs.Length; i++)
            {
                vs[i] *= ratio;
            }
            m.vertices = vs;
            mf.sharedMesh = m;
        }
        public static Vector3[] ApplyScale(this MeshFilter mf, bool copy = true)
        {
            var m = mf.sharedMesh;
            if (copy) m = Object.Instantiate(m);
            var vs = m.vertices;
            for (int i = 0; i < vs.Length; i++)
            {
                vs[i] = mf.transform.TransformPoint(vs[i]);
            }
            m.vertices = vs;
            mf.ResetTransform();
            mf.sharedMesh = m;
            return vs;
        }
        public static void RemoveSkin(this SkinnedMeshRenderer smr)
        {
            smr.rootBone = null;
            smr.bones = null;
            smr.sharedMesh.bindposes = null;
            smr.sharedMesh.boneWeights = null;
        }
    }
}