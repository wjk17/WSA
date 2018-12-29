using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
public class SetParent_ByName : MonoBehaviour
{
    public Transform[] childs;
    public Transform parent;
    public string parentName;
    public Vector3 localPosition;
    public bool worldPositionStays = true;
    public bool doOnAwake = true;
    private void Reset()
    {
        childs = new Transform[] { transform };
    }
    [Button("Clear")]
    void Clear()
    {
        var list = new List<Transform>();
        foreach (var child in childs)
        {
            if (child != null) list.Add(child);
        }
        childs = list.ToArray();
    }
    [Button("SetParent")]
    void Awake()
    {
        if (!doOnAwake) return;
        var p = parent.Search(parentName);
        foreach (var child in childs)
        {
            child.SetParent(p, worldPositionStays);
            child.localPosition = localPosition;
        }
    }
}
