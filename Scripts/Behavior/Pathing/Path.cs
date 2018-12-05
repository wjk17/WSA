using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Esa.Pathing
{
    public class Path
    {
        public List<Vector3> ps;
        public Vector3 nextPos;
        public int accuracy = 100;
        public Path()
        {
            ps = new List<Vector3>();
        }
        public bool InPath(Vector3 footPos)
        {
            var factor = 1f / accuracy;
            Vector2 minV;
            float minDist = float.PositiveInfinity;
            for (int i = 1; i < ps.Count; i++)
            {
                for (int a = 0; a < accuracy; a++)
                {
                    var p = Vector2.Lerp(ps[i - 1], ps[i], a * factor);
                    var dist = Vector2.Distance(p, footPos);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        minV = p;
                    }
                }
            }
            return false;// footPos
        }

        internal Vector3 GetNearstPoint(Vector3 footPos)
        {
            if (ps.Empty()) return Vector3.zero;
            int min = 0;
            float minDist = float.PositiveInfinity;
            for (int i = 0; i < ps.Count; i++)
            {
                if (Vector2.Distance(ps[i], footPos) < minDist)
                {
                    min = i;
                }
            }
            nextPos = ps[min];
            return nextPos;
        }
    }
}
