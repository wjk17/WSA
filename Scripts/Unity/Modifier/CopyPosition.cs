using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(CopyPosition))]
public class CopyPositionEditor : E_ShowButtons<CopyPosition> { }
#endif
[ExecuteInEditMode]
public class CopyPosition : MonoBehaviour
{
    [Range(0, 1)]
    public float weight = 1f;
    public bool lateUpdate;
    public bool update = true;
    public bool fixedUpdate;
    public bool editorUpdate;
    public Bool2 x;
    public Bool2 y;
    public Bool2 z;
    Vector3 originPos;
    public Transform target;
    void Start()
    {
        x.bool2Label = y.bool2Label = z.bool2Label = "翻转";
        originPos = transform.position;
    }
    private void LateUpdate()
    {
        if (lateUpdate) DoUpdate();
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (Selection.activeGameObject == gameObject) return;
        if (!Application.isPlaying && editorUpdate) DoUpdate();
        else if (Application.isPlaying && update) DoUpdate();
#else
        if (update) DoUpdate();
#endif
    }
    private void FixedUpdate()
    {
        if (fixedUpdate) DoUpdate();
    }
    [ShowButton]
    public void DoUpdate()
    {
        var posA = originPos;
        var posB = target.position;
        float b;
        if (x.bool1)
        {
            b = x.bool2 ? posB.x : -posB.x;
            transform.SetLocalPosX(Mathf.Lerp(posA.x, b, weight));
        }
        if (y.bool1)
        {
            b = y.bool2 ? posB.y : -posB.y;
            transform.SetLocalPosX(Mathf.Lerp(posA.y, b, weight));
        }
        if (z.bool1)
        {
            b = z.bool2 ? posB.z : -posB.z;
            transform.SetLocalPosZ(Mathf.Lerp(posA.z, b, weight));
        }
    }
}
