using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class CycleTest : MonoBehaviour {
    /// <summary>
    /// 挂载时运行，唯一一个不需ExecuteInEditMode，以及只在Editor使用的事件
    /// </summary>
    void Reset()
    {
        print("Reset");
    }
    void OnEnable()
    {
        print("OnEnable");
    }
    void Awake()
    {
        print("Awake");
    }
    void Start()
    {
        print("Start");
    }
    void Update()
    {
        print("Update");
    }
}
