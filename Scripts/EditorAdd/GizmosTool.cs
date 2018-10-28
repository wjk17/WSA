using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosTool
{
    public static void DrawSpheres(float radius, bool solid, IList<Vector3> vs)
    {
        foreach (var v in vs)
        {
            if (solid) Gizmos.DrawSphere(v, radius);
            else Gizmos.DrawWireSphere(v, radius);
        }
    }
    public static void DrawSpheres(float radius, bool solid, Color color, params Vector3[] vs)
    {
        Gizmos.color = color;
        DrawSpheres(radius, solid, vs);
    }
    public static void DrawSpheres(float radius, bool solid, params Vector3[] vs)
    {
        foreach (var v in vs)
        {
            if (solid) Gizmos.DrawSphere(v, radius);
            else Gizmos.DrawWireSphere(v, radius);
        }
    }
}
