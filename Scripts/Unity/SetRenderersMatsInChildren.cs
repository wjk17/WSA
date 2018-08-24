using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SetRenderersMatsInChildren))]
[CanEditMultipleObjects]
public class SetRenderersMatsInChildrenEditor : E_ShowButtons<SetRenderersMatsInChildren>
{

}
#endif
public class SetRenderersMatsInChildren : MonoBehaviour
{
    public Material matToReplace;
    [ShowButton]
    void SetMatsInChildren()
    {
        MatsTool.SetMatsInChildren(transform, matToReplace);
    }
}

public static class MatsTool
{
    [ShowButton]
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
