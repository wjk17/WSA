using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
public class SetRenderersMatsInChildren : MonoBehaviour
{
    public Material matToReplace;
    [Button]
    void SetMatsInChildren()
    {
        MatsTool.SetMatsInChildren(transform, matToReplace);
    }
}

public static class MatsTool
{
    [Button]
    public static void SetMatsInChildren(this Transform t, Material matToReplace)
    {
        foreach (var rdr in t.GetComponentsInChildren<Renderer>(true))
        {
            var list = new List<Material>();
            foreach (var mat in rdr.sharedMaterials)
            {
                list.Add(matToReplace);
            }
            rdr.sharedMaterials = list.ToArray();
        }
    }
}
