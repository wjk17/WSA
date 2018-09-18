using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class Chain
{
    public List<Transform> ps;
    public List<Vector3> restChildPos;
    public List<Vector3> restDir;
    public List<float> restLength;
    public List<Vector3> prevPos;
    public static Vector3 GetRelaPos(Transform tParent, Transform tChild)
    {
        //var b = tParent.InverseTransformPoint(tChild.position);
        var b = tChild.position - tParent.position;
        return b;
    }
    public static Vector3 GetRelaPos(Transform tParent, Vector3 childPos)
    {
        var b = tParent.InverseTransformPoint(childPos);
        return b;
    }
    public static Vector3 GetAbsPos(Transform tParent, Vector3 childPos)
    {
        var b = tParent.TransformPoint(childPos);
        return b;
    }
    public Chain(List<Transform> list)
    {
        ps = list;
        ps.Add(list.Last().GetChild(0)); // 加上尾巴

        prevPos = new List<Vector3>();

        restChildPos = new List<Vector3>();
        restDir = new List<Vector3>();
        restLength = new List<float>();
        for (int i = 1; i < ps.Count; i++)
        {
            var a = ps[i - 1].position;
            var b = Chain.GetRelaPos(ps[i - 1], ps[i]);

            prevPos.Add(ps[i].position);

            //restChildPos.Add(ps[i - 1].InverseTransformPoint(ps[i].position));
            var c = Quaternion.Inverse(ps[i - 1].rotation) * b;
            restChildPos.Add(c);

            restDir.Add(Quaternion.Inverse(ps[i - 1].rotation) * b.normalized);
            //restDir.Add(ps[i - 1].InverseTransformDirection(b.normalized));

            restLength.Add(b.magnitude);
        }
        ps.Last().SetParent(ps[0].parent, true);
    }
}
