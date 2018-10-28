using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Curve3Edit))]
[CanEditMultipleObjects]
public class Curve3EditEditor : E_ShowButtons<Curve3Edit> { }
#endif
public partial class Curve3Edit : MonoSingleton<Curve3Edit>
{
    [ShowToggle("编辑曲线")]
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
        else Init();
    }
    public bool useSelector = true;
    public Key3 keySelected;
    public int idxSelected;
    public int idx;
    private void LateUpdate()
    {
        if (editCurve && keySelected != null)
        {
            var k = keySelected;
            var i = idxSelected;

            var rl = HairGen.I.hairCurr.curveData.relaHairs;
            var setP = HairGen.I.SetToPivot;
            if (setP && rl.NotEmpty() && idx == 0 && i == 0) return; // 第0个锚点由HairGen控制
            if (i == 0) k.SetVector(GizmosAxis.I.transform.position);
            else if (i == 1) k.inTangent = GizmosAxis.I.transform.position;
            else if (i == 2) k.outTangent = GizmosAxis.I.transform.position;
        }
    }
    void Init()
    {
        if (useSelector)
        {
            if (Selector.current == null) { editCurve = false; return; }
            curve = Selector.current.GetComponent<Hair>().curveData.curve;
        }
        else
        {
            if (curve == null || curve.Count == 0) { editCurve = false; return; }
        }

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
