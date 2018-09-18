using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GizmosAxis
{
    public enum Axis
    {
        x, y, z
    }
    public Transform[] handles;
    public Transform[] planes;
    public Transform[] axes;
    public Material[] mats;
    public Material selected;
    public Material unSelected;
    public bool showPlane;
    public bool showAxes;
    public int axisIndex;
    public bool dragging;
    public bool hovering;

    public Vector3 downPosAxisWorld;
    Vector3 downPosWorld;

    internal Vector3 deltaPosition;
    float originSize;
    Vector3 originScale;

    string layerName = "Gizmos";
    LayerMask mask { get { return LayerMask.GetMask(layerName); } }
    private void Awake()
    {
        foreach (var t in this.GetTransforms())
        {
            t.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
    public Transform controlObj
    {
        set
        {
            _controlObj = value;
            if (value != null) transform.position = value.position;
        }
        get { return _controlObj; }
    }
    [SerializeField]
    private Transform _controlObj;
    void SetMats(Transform t, Material mat)
    {
        foreach (var render in t.GetComponentsInChildren<Renderer>())
        {
            render.material = mat;
        }
    }
    //public void SetVisible(bool visible)
    //{
    //    var renderers = GetComponentsInChildren<Renderer>(true);
    //    foreach (var renderer in renderers)
    //    {
    //        renderer.enabled = visible;
    //    }
    //}
    CameraController cc { get { return FindObjectOfType<CameraController>(); } }
    Camera cam { get { return cc.cam; } }
    Transform camT { get { return cam.transform; } }
}
