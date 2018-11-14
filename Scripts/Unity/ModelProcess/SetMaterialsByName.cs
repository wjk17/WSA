using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
public class SetMaterialsByName : MonoBehaviour 
{
    public List<Material> mats;
    [Button]
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
