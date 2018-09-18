using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR && CINEMACHINE
using Cinemachine;
[ExecuteInEditMode]
public class FollowSceneView : MonoBehaviour
{
    public bool on;
    public Transform cam;
    public CinemachineVirtualCamera vCam;
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
        if (vCam != null)
        {
            var lens = vCam.m_Lens;
            lens.NearClipPlane = scnCam.nearClipPlane;
            lens.FarClipPlane = scnCam.farClipPlane;
            lens.FieldOfView = scnCam.fieldOfView;
            //lens.Orthographic = scnCam.orthographic;
            lens.OrthographicSize = scnCam.orthographicSize;
            vCam.m_Lens = lens;
            vCam.transform.position = scnCam.transform.position;
            vCam.transform.rotation = scnCam.transform.rotation;
        }
    }
}
#endif
