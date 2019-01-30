using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Esa;
using Esa.UI_;
[Serializable]
public class Flag
{
    public bool on;
    public string name;
}
[Serializable]
public class ChatData
{
    public List<string> text;
}
public class NPC : MonoBehaviour
{
    public static string layerName = "NPC";
    public static LayerMask layerMask { get { return LayerMask.GetMask(layerName); } }
    public static int layerNum { get { return (int)Mathf.Log(layerMask.value, 2); } }
    // flags
    public List<Flag> flags;
    public List<ChatData> datas;
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
}
