using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SetParent))]
public class SetParentEditor : E_ShowButtons<SetParent> { }
#endif
public class SetParent : MonoBehaviour
{
    public Transform[] childs;
    public Transform parent;
    public bool worldPositionStays = true;
    private void Reset()
    {
        childs = new Transform[] { transform };
    }
    [ShowButton("Clear")]
    void Clear()
    {
        var list = new List<Transform>();
        foreach (var child in childs)
        {
            if (child != null) list.Add(child);
        }
        childs = list.ToArray();
    }
    [ShowButton("SetParent")]
    void Awake()
    {
        foreach (var child in childs)
        {
            child.SetParent(parent, worldPositionStays);
        }
    }
}
