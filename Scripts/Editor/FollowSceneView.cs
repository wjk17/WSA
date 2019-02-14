﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class FollowSceneView : MonoBehaviour
{
    public bool on;
    public Transform cam;
    public void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            Follow();
    }
    void Follow()
    {
        var sceneViews = UnityEditor.SceneView.sceneViews;
        if (!on || sceneViews.Count == 0) return;

        var sceneView = (UnityEditor.SceneView)sceneViews[0];
        var scnCam = sceneView.camera;
        if (cam != null)
        {
            cam.position = scnCam.transform.position;
            cam.rotation = scnCam.transform.rotation;
        }
        var c = cam.GetComponent<Camera>();
        if (c != null)
        {
            c.nearClipPlane = scnCam.nearClipPlane;
            c.farClipPlane = scnCam.farClipPlane;
            c.fieldOfView = scnCam.fieldOfView;
            c.orthographicSize = scnCam.orthographicSize;
        }
    }
}
#endif
