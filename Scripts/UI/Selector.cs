using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Selector : MonoSingleton<Selector>
{
    [Header("ReadOnly")]
    public Transform Current;
    public static Transform current;
    public static Transform prev;
    public LayerMask mask;
    public Action onClick;
    private void Start()
    {
        UI.I.AddInputCB(name, GetInput, 0);
    }
    void GetInput()
    {
        if (!gameObject.activeSelf || !enabled) return;
        if (Events.MouseUp(1))
        {
            Transform target;
            var hit = this.SVRaycast(Input.mousePosition, out target, mask.value);
            if (hit)
            {
                prev = current;
                current = target;
                if (onClick != null) onClick();
            }
        }
        Current = current;
    }
}
