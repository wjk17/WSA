using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
[ExecuteInEditMode]
public class TestRayCast : MonoBehaviour
{
    public float len = 1;
    public float t;
    public Transform plane;
    public Transform ray;
    private void Reset()
    {
        plane = transform.GetChild(0);
        ray = transform.GetChild(1);
    }
    void Update()
    {
        var n = plane.up;
        ray.SetLocalScaleY(len);

        var u = ray.up; // direction
        var p0 = ray.position;
        //var p = p0 + t * u;
        var p1 = plane.position;

        var a = Vector3.Dot(n, p1);
        var b = Vector3.Dot(n, p0);
        var c = Vector3.Dot(n, u);
        t = (a - b) / c;
    }
}
