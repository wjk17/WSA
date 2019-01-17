using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class AutoRigging : Singleton<AutoRigging>
    {
        [Serializable]
        public class ClosestInfo
        {
            public ClosestInfo()
            {
                dist = float.PositiveInfinity;
            }
            public float dist;
            public Transform bone;
            public static int Compare(ClosestInfo x, ClosestInfo y)
            {
                return y.dist.CompareTo(x.dist); // 使Sort排序时倒序
                                                 ///return x.dist.CompareTo(y.dist);
            }
        }
        public static float DistTotal(IList<ClosestInfo> closests)
        {
            float dist = 0f;
            foreach (var info in closests)
            {
                dist += info.dist;
            }
            return dist;
        }
        public int maxBoneCount = 4; // 每个顶点最多受到多少跟骨骼的影响
        public float powValue = 2;
        public AnimationCurve curveWeight;
        // 距离权重刷一波
        public static List<BoneWeights> AutoRigDist(Transform rig, Mesh mesh, Transform[] bones)
        {
            var boneWgts = new List<BoneWeights>();
            var verts = mesh.vertices;
            var bwCount = Mathf.Min(I.maxBoneCount, bones.Length);
            var closests = new List<ClosestInfo>();
            //mesh.vertexCount diff to vertices.Length???
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                ListTool.ConstructList(ref closests, bwCount);
                //            verts[i] = transform.TransformPoint(verts[i]);
                for (int b = 0; b < bones.Length; b++)
                {
                    for (int w = 0; w < bwCount; w++) //最大骨骼数量，先保存这个数量的最近的骨骼
                    {
                        var info = closests[w];
                        var dist = Vector3.Distance(bones[b].position, verts[i]);
                        if (dist < info.dist) // 假如有四个骨骼，距离少于任意一个骨骼都
                        {
                            info.bone = bones[b];
                            info.dist = dist;
                            //sort
                            closests.Sort(ClosestInfo.Compare);
                            break;
                        }
                    }
                }
                var bi = new BoneWeights();
                boneWgts.Add(bi);
                //var distTotal = Mathf.Pow(DistTotal(), 2);
                var distTotal = DistTotal(closests);
                float n;
                for (int j = 0; j < closests.Count; j++)
                {
                    Weight bw = new Weight();
                    bi.wgts.Add(bw);
                    bw.t = closests[j].bone;
                    //bw.weight = Mathf.Pow(closests[j].dist, 2) / distTotal;
                    //if(closests[j].dist ==0) bw.weight
                    //    else bw.weight = closests[j].dist / distTotal;
                    if (closests[j].dist == 0) n = 0;
                    else n = closests[j].dist / distTotal;
                    bw.weight = 1 - n;
                    bw.weight = Mathf.Pow(bw.weight, I.powValue);
                    if (float.IsNaN(bw.weight)) Debug.Log("Nan");
                    bw.delta = bw.t.InverseTransformPoint(verts[i]);// 顶点相对于骨骼的本地坐标
                }
                BoneWeights.NormalizeWeight(bi);
                foreach (var w in bi.wgts)
                {
                    w.weight = I.curveWeight.Evaluate(w.weight);
                }
                BoneWeights.NormalizeWeight(bi);
                foreach (var w in bi.wgts)
                {
                    if (float.IsNaN(w.weight)) Debug.Log("Nan After Curve And Normalize");
                }
            }
            //print("boneWgts.Count:" + boneWgts.Count + Environment.NewLine + " " +
            //    MeshTool.ToString(mesh) + Environment.NewLine);
            rig.Rotate(Vector3.right, -90);
            rig.ApplyTransform();
            //RigCurveModifier
            return boneWgts;
        }
    }
}