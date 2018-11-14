﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
public class HideLayer : MonoBehaviour
{
    public List<HideOnAwake> list;  
    [Button]
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
