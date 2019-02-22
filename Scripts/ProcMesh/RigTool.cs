using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa
{
    [Serializable]
    public class RigData
    {
        public int bonesCount;
        public Curve2 curveBoneLength;
        public float length;
    }
    public class RigTool : MonoBehaviour
    {
        public static void CreateRig(RigData info, Transform rig, out Transform[] bones)
        {
            ComTool.ClearChildren(rig);
            float totalLength = 0;
            var ls = new float[info.bonesCount];
            float len;
            for (int i = 0; i < info.bonesCount; i++)
            {
                len = info.curveBoneLength.Evaluate1D(i / (float)info.bonesCount);
                totalLength += len;
                ls[i] = len;
            }
            var ratio = info.length / totalLength;
            Transform p = null;
            for (int i = 0; i < info.bonesCount; i++)
            {
                var bone = new GameObject("bone " + i.ToString()).transform;
                bone.parent = p == null ? rig : p;
                p = bone;
                if (i == 0) bone.SetLocalPosY(0);
                else bone.SetLocalPosY(ls[i - 1] * ratio);
            }
            bones = rig.GetTransforms().ToArray();

            var tail = new GameObject("bone tail");
            tail.SetParent(rig);
            tail.SetLocalPosY(info.length);
            tail.SetParent(p, true);

            // flat bones
            foreach (var bone in bones)
            {
                bone.parent = rig;
            }
        }
    }
}