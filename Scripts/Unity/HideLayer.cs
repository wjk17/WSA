using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(HideLayer))]
public class HideLayerEditor : E_ShowButtons<HideLayer> { }
#endif
public class HideLayer : MonoBehaviour
{
    public List<HideOnAwake> list;  
    [ShowButton]
    void HaveALook()
    {
        list = TransformTool.GetComsScene<HideOnAwake>();
    }
    void Awake()
    {
        list = TransformTool.GetComsScene<HideOnAwake>();
        foreach (var hoa in list)
        {
            hoa.SetParent(transform);
        }
        gameObject.SetActive(false);
    }
}
