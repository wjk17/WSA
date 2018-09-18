using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsChain : MonoBehaviour
{
    public List<Transform> rigs;
    public List<Chain> chains;
    public int boneCount;

    public float stiffness = 20;
    public float drag = 0.5f;
    public float mass = 1;
    [Range(-1, 0f)]
    public float gravity = -0.05f;
    //[Range(-10, 10f)]
    public float windX = 0f;
    public AnimationCurve windCurve;
    public float timeFactor = 1;
    public bool limitLength = true; // 每帧即时还原长度
    public bool diffLength;
    public bool useDiff = true; // 收敛
    public bool lookAtChild = true;
    public float verletFactor = 0.3f;
    public bool useVerlet = true;

    public float len = 0; // debug动量的线    

    [ContextMenu("Start")]
    void Start()
    {
        rigs = this.GetTrans("Rig", false);
        chains = new List<Chain>();
        boneCount = 0;
        foreach (var rig in rigs)
        {
            var ts = rig.GetTransforms();
            if (ts.Count < 2) Debug.LogError("child less then 2");
            chains.Add(new Chain(ts.RemoveLast()));
            boneCount += chains.Last().ps.Count - 1;
        }
    }
    void Resolve(Chain C)
    {
        for (int j = 1; j < C.ps.Count; j++)
        {
            var i = j - 1;
            var pA = C.ps[i];
            var pB = C.ps[j];
            var pos = pB.position - pA.position;
            //var pos = Chain.GetRelaPos(pA, pB);

            var dragF = (C.prevPos[i] != pB.position) ? (C.prevPos[i] - pB.position).normalized * drag : Vector3.zero;

            //var r = pA.TransformPoint(C.restChildPos[i]) - pB.position;
            var r = pA.rotation * C.restChildPos[i] - pos;

            float diff;
            if (diffLength)
                diff = pos.magnitude - C.restLength[i];
            else diff = r.magnitude;
            var dir = r.normalized; //还原的方向
            var F = dir * stiffness * (useDiff ? diff : 1);
            F += dragF;
            F += Vector3.up * gravity;
            //F += Vector3.right * windX * windCurve.Evaluate(Time.deltaTime * timeFactor);
            F = F / mass * Time.deltaTime;
            pos += F;

            if (useVerlet)
            {
                var verlet = pB.position - C.prevPos[i];
                pos += verlet * verletFactor;
            }

            Vector3 d;
            if (limitLength)///限定长度
            {
                //防止normalized零矢量出错
                d = (pos == Vector3.zero) ? pA.TransformDirection(C.restDir[i]) : pos.normalized;
                pos = d * C.restLength[i];
            }
            //pB.position = pA.TransformPoint(pos);
            pB.position = pA.position + pos;
            C.prevPos[i] = pB.position;

            if (lookAtChild)
            {

                var a = pos.normalized;
                var b = pA.rotation * C.restDir[i];
                //var b = pA.TransformDirection(C.restDir[i]);

                Debug.DrawRay(pA.position, b * len * 1.5f, Color.blue, 0, false);
                Debug.DrawRay(pA.position, a * len, Color.red, 0, false);

                var rot = Quaternion.FromToRotation(b, a);

                if (j == C.ps.Count - 1)
                {
                    var p = pB.position;
                    pA.rotation = rot * pA.rotation;
                    pB.position = p;
                }
                else pA.rotation = rot * pA.rotation;
            }
        }
    }
    void Update()
    {
        foreach (var chain in chains)
        {
            Resolve(chain);
        }
    }
}
