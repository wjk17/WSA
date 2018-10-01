using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(MonoSingletonMgr))]
public class MonoSingletonMgrEditor : E_ShowButtons<MonoSingletonMgr> { }
#endif
public class MonoSingletonMgr : MonoBehaviour
{
    [ShowButton]
    void Awake()
    {
        foreach (var obj in FindObjectsOfType<MonoSingletonBase>())
        {
            obj.Init();
        }
    }
}
