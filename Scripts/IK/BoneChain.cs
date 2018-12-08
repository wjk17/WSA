using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
namespace Esa
{
    [Serializable]
    public class BoneChain // IK骨骼链
    {
        public List<Joint> joints;
        public Transform end;
        public Transform target;
        public int iteration; // 计算的迭代次数
        public List<WRay> Solve()
        {
            List<WRay> result = new List<WRay>();
            int i;
            for (i = 0; i < iteration; i++)
            {
                foreach (var joint in joints)
                {
                    var ray = IKTool.IKSolve(joint, target, end);
                    if (ray != null) result.Add(ray);
                }
                //if (IKTool.Close(target.position, end.position)) { Debug.Log(i.ToString() + " 次迭代   完成"); return; }
                if (IKTool.Close(target.position, end.position)) return result;
            }
            return result;
            //Debug.Log(i.ToString() + " 次迭代  未完成");
        }
        public WRay Solve(int i)
        {
            var result = IKTool.IKSolve(joints[i], target, end);
            if (IKTool.Close(target.position, end.position)) Debug.Log("完成");
            return result;
        }
    }
}

