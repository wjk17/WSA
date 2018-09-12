using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Curve3
{
    Vector3 Evaluate(Key3 a, Key3 b, float t)
    {
        var points = new Vector3[4];
        points[0] = a.vector;
        points[1] = a.outTangent;
        points[2] = b.inTangent;
        points[3] = b.vector;
        var Points3 = new Vector3[3];
        for (int i = 0; i < 3; i++)
        {
            Points3[i] = Vector3.Lerp(points[i], points[i + 1], t);
        }
        var Points2 = new Vector3[2];
        for (int i = 0; i < 2; i++)
        {
            Points2[i] = Vector3.Lerp(Points3[i], Points3[i + 1], t);
        }
        return Vector3.Lerp(Points2[0], Points2[1], t);
    }
    public Vector3 Evaluate(float t)
    {
        var i = 0;
        for (; i < listEndTime.Count; i++)
        {
            if (t < listEndTime[i])
            {
                goto findIndex;
            }
        }
        return keys[Count - 1].vector;
        findIndex:
        var tLocal = t - listStartTime[i];
        var tN = tLocal / (listEndTime[i] - listStartTime[i]);
        return Evaluate(this[i], this[i + 1], tN);
    }
    float Slope(Key3 a, Key3 b, float t)
    {
        return Mathf.Lerp(a.slope, b.slope, t);
    }
    public float Slope(float t)
    {
        var i = 0;
        for (; i < listEndTime.Count; i++)
        {
            if (t < listEndTime[i])
            {
                goto findIndex;
            }
        }
        return keys[Count - 1].slope;
        findIndex:
        var tLocal = t - listStartTime[i];
        var tN = tLocal / (listEndTime[i] - listStartTime[i]);
        return Slope(this[i], this[i + 1], tN);
    }
}
