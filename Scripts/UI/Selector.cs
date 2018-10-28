using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Selector))]
public class E_Selector : E_ShowButtons<Selector> { }
#endif
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
        UI.I.AddInputCB(name, GetInput, 0);
    }
    [ShowButton]
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
