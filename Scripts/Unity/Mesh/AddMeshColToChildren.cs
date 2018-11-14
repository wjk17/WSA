using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
public class AddMeshColToChildren : MonoBehaviour
{
    public bool doOnStart;
    [Button]
    void AddMeshColliderToChildren()
    {
        var rdrs = GetComponentsInChildren<Renderer>();
        Mesh mesh = null;
        foreach (var rdr in rdrs)
        {
            if (!rdr.enabled) continue;
            var mf = rdr.GetComponent<MeshFilter>();
            if (mf != null)
            {
                mesh = mf.sharedMesh;
            }
            else
            {
                var smr = rdr as SkinnedMeshRenderer;
                if (smr != null)
                {
                    smr.BakeMesh(mesh);
                    var vs = mesh.vertices;
                    var list = new List<Vector3>();
                    foreach (var v in vs)
                    {
                        //list.Add(rdr.transform.InverseTransformPoint(v));
                        var s = VectorTool.Divide(Vector3.one, rdr.transform.localScale);
                        list.Add(Vector3.Scale(s, v));
                    }
                    mesh.vertices = list.ToArray();
                }
            }
            var mc = rdr.GetComOrAdd<MeshCollider>();
            mc.sharedMesh = mesh;
            //mc.convex = true;
        }
    }
    [Button]
    void ClearMeshColliders()
    {
        foreach (var mc in GetComponentsInChildren<MeshCollider>(true))
        {
            ComTool.DestroyAuto(mc);
        }
    }
    private void Start()
    {
        if (doOnStart) AddMeshColliderToChildren();
    }
}
