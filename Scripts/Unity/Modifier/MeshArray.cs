using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(MeshArray))]
public class MeshArrayEditor : E_ShowButtons<MeshArray> { }
#endif
[ExecuteInEditMode]
public class MeshArray : MonoBehaviour
{
    public Vector3 offset;
    private Vector3 _offset;
    public Vector3 scale;
    private Vector3 _scale;
    public Vector3 offsetStart;
    private Vector3 _offsetStart;
    public int count;
    private int _count;
    public Mesh mesh;
    public Mesh meshOrigin;
    public AnimationCurve curve;
    private AnimationCurve _curve;
    public AnimationCurve curveScale;
    private AnimationCurve _curveScale;
    public bool log = true;
    public bool doOnAwake = false;

    public List<Vector3> spacings;

    void GetOrigin()
    {
        meshOrigin = GetComponent<MeshFilter>().sharedMesh;
    }
    void Awake()
    {
        if (doOnAwake) UpdateMesh();
    }
    [ShowButton]
    void UpdateMesh()
    {
        if (meshOrigin == null) GetOrigin();
        mesh = new Mesh();
        mesh.name = "Array Mesh";

        var vs = meshOrigin.vertices;
        var vl = new List<Vector3>();
        var ns = meshOrigin.normals;
        var nl = new List<Vector3>();
        var ts = meshOrigin.triangles;
        var tl = new List<int>();
        spacings = new List<Vector3>();
        var osCurr = Vector3.Scale(offsetStart, offset);
        for (int i = 0; i < count; i++)
        {
            var t = count == 1 ? 0 : i / (float)(count - 1);
            if (i != 0) osCurr += curve.Evaluate(t) * offset;
            spacings.Add(transform.TransformVector(osCurr));

            var scaleFactor = curveScale.Evaluate(t);
            foreach (var v in vs)
            {
                var scaled = Vector3.Scale(v, scale) * scaleFactor;
                vl.Add(scaled + osCurr);
            }
            foreach (var n in ns)
            {
                nl.Add(n);
            }
            foreach (var tri in ts)
            {
                tl.Add(tri + vs.Length * i);
            }
        }
        mesh.vertices = vl.ToArray();
        mesh.normals = nl.ToArray();
        mesh.triangles = tl.ToArray();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        if (log) Debug.Log("Update Mesh");
    }
    bool Equals(AnimationCurve a, AnimationCurve b)
    {
        if ((a == null) != (b == null)) return false;
        if (a.length != b.length) return false;
        for (int i = 0; i < a.length; i++)
        {
            if (!a.keys[i].time.Approx(b.keys[i].time, 3) ||
                !a.keys[i].value.Approx(b.keys[i].value, 3)) return false;
        }
        return true;
    }
    AnimationCurve Copy(AnimationCurve curve)
    {
        if (curve == null) return null;
        return new AnimationCurve(curve.keys);
    }
    void Update()
    {
        if (_offset != offset || _count != count ||
            _scale != scale || _offsetStart != offsetStart ||
            Equals(_curve, curve) == false ||
            Equals(_curveScale, curveScale) == false)
        {
            _offset = offset;
            _count = count;
            _scale = scale;
            _offsetStart = offsetStart;
            _curve = Copy(curve);
            _curveScale = Copy(curveScale);
            UpdateMesh();
        }
    }
}
