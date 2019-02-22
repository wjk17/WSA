using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class DirectionalLightPosControl : MonoBehaviour
{
    public Material mat;
    public Transform target;
    Transform prevTarget;
    public Vector3 weight;
    //public Transform child;
    public bool update;
    public string propNameDir = "_BaseCustomDir";
    public string propNameWeight = "_CustomDirWeight";
    private void OnEnable()
    {
        GetProps();
    }
    private void Start()
    {
        GetProps();
    }
    void GetProps()
    {
        Vector3 dir = mat.GetVector(propNameDir);
        dir *= (transform.position - target.position).magnitude;
        transform.position = target.position + dir;
        //weight = mat.GetVector(propNameWeight);
    }
    void SetLightDir(Vector3 dir)
    {
        dir = dir.normalized;
        mat.SetVector(propNameDir, dir);
        transform.GetChild(0).forward = -dir;
    }
    void SetWeight(Vector3 f)
    {
        mat.SetVector(propNameWeight, f);
    }
    void Update()
    {
        if (!update || target == null) return;

        if (prevTarget != target)
        {
            GetProps();
            prevTarget = target;
        }

        SetLightDir(transform.position - target.position);
        //SetWeight(weight);
    }
}
