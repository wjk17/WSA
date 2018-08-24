using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Use GetInstance() when start the scene
/// </summary>
public class MonoSingleton<TMono> : MonoBehaviour where TMono : MonoBehaviour
{
    /// <summary>
    /// 如果需要在场景开始时禁用单例对象，使用这个字段来禁用，否则会有找不到单例的可能出现。
    /// </summary>
    public bool startActive = true;
    public void GetInstance()
    {
        if (startActive == false) gameObject.SetActive(false);
        _i = (TMono)(object)this;
    }
    public static TMono I
    {
        get { if (_i == null) _i = FindObjectOfType<TMono>(); return _i; }
    }
    private static TMono _i;

    public static Transform T
    {
        get { return I.transform; }
    }
}

public class MonoBehaviourSafeCall<T> : MonoBehaviour where T : MonoBehaviour
{
    public void SafeCall(Action act)
    {
        if (act != null) act();
    }
}

public class MonoBehaviourTools<T1> : MonoBehaviour where T1 : MonoBehaviour
{
    public static T1 I
    {
        get { if (_i == null) _i = FindObjectOfType<T1>(); return _i; }
    }
    private static T1 _i;

    public static Transform T
    {
        get { return I.transform; }
    }
    public void SafeCall(Action act)
    {
        if (act != null) act();
    }
    public void SafeCall(Action<int> act, int i)
    {
        if (act != null) act(i);
    }
}
