using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SetMaterialsByName))]
public class SetMaterialsByNameEditor : E_ShowButtons<SetMaterialsByName> { }
#endif
public class SetMaterialsByName : MonoBehaviour 
{
    public List<Material> mats;
    [ShowButton]
    void setMaterialsByName()
    {
        foreach (var rdr in GetComponentsInChildren<Renderer>(true))
        {
            var newMats = new List<Material>();
            foreach (var matO in rdr.sharedMaterials)
            {
                var idx = mats.IdxOf(matO.name);
                if (idx > -1) newMats.Add(mats[idx]);
                else newMats.Add(matO);
            }
            rdr.sharedMaterials = newMats.ToArray();
        }
    }
}
