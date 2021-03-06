﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
public class SetParent : MonoBehaviour
{
    public Transform[] childs;
    public Transform parent;
    public bool worldPositionStays = true;
    public bool doOnAwake = true;
    public bool doOnStart = true;
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
    void DoSetParent()
    {        
        foreach (var child in childs)
        {
            child.SetParent(parent, worldPositionStays);
        }
    }
    private void Awake()
    {
        if (doOnAwake) DoSetParent();
    }
    private void Start()
    {
        if (doOnStart) DoSetParent();
    }
}
