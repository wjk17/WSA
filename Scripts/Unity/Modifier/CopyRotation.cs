using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(CopyRotation))]
public class CopyRotationEditor : E_ShowButtons<CopyRotation> { }
#endif
[ExecuteInEditMode]
public class CopyRotation : MonoBehaviour
{
    [Range(0, 1)]
    public float weight = 1f;
    public bool lateUpdate;
    public bool update = true;
    public bool fixedUpdate;
    public bool editorUpdate;

    Quaternion originRot;
    Quaternion originRotTarget;
    public Transform target;
    void Start()
    {
        originRot = transform.rotation;
        originRotTarget = target.rotation;
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
        var rot = target.rotation * Quaternion.Inverse(originRotTarget);
        rot = Quaternion.Lerp(Quaternion.identity, rot, weight);
        transform.rotation = rot * originRot;
    }
}
