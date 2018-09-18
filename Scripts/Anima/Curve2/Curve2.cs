using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Curve2
{
    /// <summary>
    /// add key and Sort
    /// </summary>
    /// <param name="key"></param>
    public void InsertKey(Key2 key) // 插入帧并且立即排序
    {
        Add(key);
        Sort();
        var i = IndexOf(key);
        float len;
        Vector2 dir;
        if (i > 0 && i < Count - 1)
        {
            var a = this[i - 1];
            var b = this[i + 1];
            //var localT = (key.time - a.time) / (b.time - a.time);
            //var t1 = Mathf.Max(0, localT - 0.2f);
            //var t2 = Mathf.Min(1, localT + 0.2f);
            //Debug.Log("LocalT: " + localT.ToString());
            //Debug.Log("t1: " + t1.ToString());
            //Debug.Log("t2: " + t2.ToString());
            //Debug.Log("v1: " + a.vector);
            //Debug.Log("v2: " + b.vector);
            //key.inTangent = Evaluate(a, b, t1);
            //key.outTangent = Evaluate(a, b, t2);
            key.inTangent = Vector2.Lerp(key.vector, a.vector, 0.33f);
            key.outTangent = Vector2.Lerp(key.vector, b.vector, 0.33f);
            var lenA = a.LocalOut();
            if (lenA != Vector2.zero)
                a.SetLocalOut(lenA.normalized * lenA.magnitude * 0.5f);
            var lenB = b.LocalIn();
            if (lenB != Vector2.zero)
                b.SetLocalIn(lenB.normalized * lenB.magnitude * 0.5f);
        }
        else if (i > 0) // i == count -1
        {
            len = (key.time - this[i - 1].time) * 0.4f;
            dir = ((this[i - 1].vector - key.vector) * Vector2.right).normalized;
            key.inTangent = key.vector + dir.normalized * len;
            key.outMode = KeyMode.None;
        }
        else if (i < Count - 1) // i == 0
        {
            len = (this[i + 1].time - key.time) * 0.4f;
            dir = ((this[i + 1].vector - key.vector) * Vector2.right).normalized;
            key.outTangent = key.vector + dir.normalized * len;
            key.inMode = KeyMode.None;
        }
        else // i == 0 and Count == 0
        {
            dir = Vector2.right * 0.4f;
            key.outTangent = key.vector + dir;
            key.inMode = KeyMode.None;
        }
    }
    void FitTangentLenOut(int i)
    {
        return;
        var k = keys[i];
        var len = (keys[i + 1].time - k.time) * 0.4f;
        var angle = Vector2.Angle(k.LocalOut(), k.vector + Vector2.right * len);
        var factor = 1f / Mathf.Cos(angle);
        keys[i].SetLengthOut(len * factor);
    }
    void FitTangentLenIn(int i)
    {
        return;
        var k = keys[i];
        var len = (k.time - keys[i].time) * 0.4f;
        var angle = Vector2.Angle(k.LocalIn(), k.vector + Vector2.left * len);
        var factor = 1f / Mathf.Cos(angle);
        keys[i].SetLengthIn(len * factor);
    }
    public void Sort()
    {
        keys.Sort(SortList);
    }
    private int SortList(Key2 a, Key2 b)
    {
        if (a.time > b.time) { return 1; }
        else if (a.time < b.time) { return -1; }
        return 0;
    }
}
