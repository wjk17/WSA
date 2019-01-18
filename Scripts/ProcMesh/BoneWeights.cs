using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
namespace Esa
{
    [Serializable]
    public class BoneWeights
    {
        public BoneWeights() { wgts = new List<Weight>(); }
        public List<Weight> wgts;
        public static void NormalizeWeight(BoneWeights bws)
        {
            var weightTotal = 0f;
            foreach (var w in bws.wgts)
            {
                weightTotal += w.weight;
            }
            var ratio = 1 / weightTotal;
            foreach (var w in bws.wgts)
            {
                w.weight *= ratio;
            }
        }
        public static Vector3 GeneratePos(BoneWeights bws)
        {
            var v = Vector3.zero;
            foreach (Weight weight in bws.wgts)
            {
                v += weight.t.TransformPoint(weight.delta) * weight.weight;
            }
            return v;
        }
    }
    [Serializable]
    public class Weight
    {
        public Transform t;
        public float weight;
        public Vector3 delta;
    }
}