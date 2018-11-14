using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Esa;
public class Selector : MonoSingleton<Selector>
{
    [Header("ReadOnly")]
    public Transform Current;
    public static Transform current;
    public static Transform prev;
    public LayerMask mask;
    public Func<Transform, Transform> onClick;
    private void Start()
    {
        this.AddInputCB(GetInput, 0);
    }
    [Button]
    public void UnSelectAll()
    {
        Select(null);
    }
    void GetInput()
    {
        if (UICurve.I.gameObject.activeSelf) return;
        if (!gameObject.activeSelf || !enabled) return;
        if (Events.MouseUp(1))
        {
            Transform target;
            var hit = this.SVRaycast(Input.mousePosition, out target, mask.value);
            if (hit)
            {
                Select(target);
            }
        }
        Current = current;
    }
    void Select(Transform target)
    {
        if (onClick != null && target != null) target = onClick(target);
        prev = current;
        current = target;
        if (prev != current) OnSelectionChanged();
    }
    public Action onSelectionChanged;
    private void OnSelectionChanged()
    {
        if (onSelectionChanged != null) onSelectionChanged();
    }
}
