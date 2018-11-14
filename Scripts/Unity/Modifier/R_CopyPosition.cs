using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
#if UNITY_EDITOR 
using UnityEditor;
#endif
[ExecuteInEditMode]
public class R_CopyPosition : MonoBehaviour
{
    public enum Space
    {
        World,
        Local,
        UI,
    }
    public Transform from;
    public Transform to;
    public bool lateUpdate;
    public bool update = true;
    public bool fixedUpdate;
    public bool editorUpdate;
    public Bool2 x;
    public Bool2 y;
    public Bool2 z;
    public Space space = Space.Local;
    private void Reset()
    {
        x.bool2Label = y.bool2Label = z.bool2Label = "翻转";
        to = transform;
    }
    private void LateUpdate()
    {
        if (lateUpdate) UpdatePos();
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (Selection.activeGameObject == gameObject) return;
        if (!Application.isPlaying && editorUpdate) UpdatePos();
        else if (Application.isPlaying && update) UpdatePos();
#else
        if (update) UpdatePos();
#endif
    }
    private void FixedUpdate()
    {
        if (fixedUpdate) UpdatePos();
    }
    [Button]
    public void UpdatePos()
    {
        if (space == Space.World)
        {
            if (x.bool1)
            {
                to.CopyPosX(from);
                if (x.bool2) to.SetPosX(-to.position.x);
            }
            if (y.bool1)
            {
                to.CopyPosY(from);
                if (y.bool2) to.SetPosY(-to.position.y);
            }
            if (z.bool1)
            {
                to.CopyPosZ(from);
                if (z.bool2) to.SetPosZ(-to.position.z);
            }
        }
        else if (space == Space.Local)
        {
            if (x.bool1)
            {
                to.CopyLocalPosX(from);
                if (x.bool2) to.SetLocalPosX(-to.localPosition.x);
            }
            if (y.bool1)
            {
                to.CopyLocalPosY(from);
                if (y.bool2) to.SetLocalPosY(-to.localPosition.y);
            }
            if (z.bool1)
            {
                to.CopyLocalPosZ(from);
                if (z.bool2) to.SetLocalPosZ(-to.localPosition.z);
            }
        }
        else if (space == Space.UI)
        {
            if (x.bool1) to.CopyUIPosX(from, x.bool2 ? -1 : 1);
            if (y.bool1) to.CopyUIPosY(from, y.bool2 ? -1 : 1);
        }
    }
}

