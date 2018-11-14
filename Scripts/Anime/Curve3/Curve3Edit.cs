using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Esa;
public partial class Curve3Edit : MonoSingleton<Curve3Edit>
{
    [Toggle("编辑曲线")]
    bool editCurve;
    public bool EditCurve
    {
        set { editCurve = value; editCurve_Changed(value); }
        get { return editCurve; }
    }
    void editCurve_Changed(bool value)
    {
        if (value == false)
        {
            if (keysContainer != null) keysContainer.ClearChildren();
            return;
        }
        else EditCurve_OnEnable();
    }
    public Key3 keySelected;
    public int idxSelected;
    public int idx;
    /// <summary>
    /// 使用 EditCurve 打开曲线编辑器之前要先给曲线字段赋值，否则不能打开。
    /// </summary>
    void EditCurve_OnEnable()
    {
        if (curve.Empty()) { editCurve = false; Debug.Log("无曲线"); return; }

        keysContainer = DrawLayer.I.Search(keysContainerName);
        if (keysContainer == null)
        {
            keysContainer = new GameObject(keysContainerName).transform;
            keysContainer.SetParent(DrawLayer.I);
        }

        int i = 0;
        foreach (var key in curve)
        {
            var go = NewGO("Key " + i.ToString(), key, 0, i++);

            if (key.inMode == KeyMode.Bezier)
            {
                go = NewGO("Key " + i.ToString() + " Out", key, 1, i);
            }
            if (key.outMode == KeyMode.Bezier)
            {
                go = NewGO("Key " + i.ToString() + " Out", key, 2, i);
            }
        }
    }
    public Func<Key3, int, int, bool> updateCheck;
    private void LateUpdate()
    {
        if (editCurve && keySelected != null)
        {
            var k = keySelected;
            var i = idxSelected;

            if (updateCheck != null && !updateCheck(k, i, idx)) return;

            if (i == 0) k.SetVector(GizmosAxis.I.transform.position);
            else if (i == 1) k.inTangent = GizmosAxis.I.transform.position;
            else if (i == 2) k.outTangent = GizmosAxis.I.transform.position;
        }
    }
    GameObject NewGO(string name, Key3 k, int i, int idx)
    {
        var go = new GameObject(name);
        var t = go.transform;
        t.gameObject.layer = layerKeyNum;
        t.SetParent(keysContainer);
        t.position = i == 0 ? k.vector : i == 1 ? k.inTangent : k.outTangent;

        var ce = go.AddComponent<ColliderEvents>();
        ce.boxSize = keyClickSize;
        ce.onRaycastHit = () => { ClickKey(k, i, idx); };
        if (i == 0)
            ce.onUpdate = () => { ce.transform.position = k.vector; };
        else if (i == 1)
            ce.onUpdate = () => { ce.transform.position = k.inTangent; };
        else
            ce.onUpdate = () => { ce.transform.position = k.outTangent; };
        return go;
    }
    void ClickKey(Key3 k, int i, int idx)
    {
        this.idx = idx;
        keySelected = k;
        idxSelected = i;
        if (i == 0) GizmosAxis.I.transform.position = k.vector;
        else if (i == 1) GizmosAxis.I.transform.position = k.inTangent;
        else if (i == 2) GizmosAxis.I.transform.position = k.outTangent;
    }
}
