using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Curve3
{
    float LenBetweenKeys(Key3 a, Key3 b)
    {
        var factor = 1f / accuracy;
        var prev = a.vector;
        float len = 0;
        for (int i = 1; i < accuracy; i++)
        {
            var curr = Evaluate(a, b, i * factor);
            len += Vector3.Distance(prev, curr);
            prev = curr;
        }
        return len;
    }
    float Length()
    {
        float len = 0;
        for (int i = 1; i < Count; i++)
        {
            len += LenBetweenKeys(this[i - 1], this[i]);
        }
        return len;
    }
    public List<float> listEndTime;
    public List<float> listStartTime;
    public float lenTotal;
    public void InitRangeTimes()
    {
        lenTotal = Length();
        float lenCurrent = 0;
        listEndTime = new List<float>();
        listStartTime = new List<float>();
        for (int i = 1; i < Count; i++)
        {
            listStartTime.Add(lenCurrent / lenTotal);
            lenCurrent += LenBetweenKeys(this[i - 1], this[i]);
            listEndTime.Add(lenCurrent / lenTotal);
        }
    }
}
