//
//SpingManager.cs for unity-chan!
//
//Original Script is here:
//ricopin / SpingManager.cs
//Rocket Jump : http://rocketjump.skr.jp/unity3d/109/
//https://twitter.com/ricopin416
//
//Revised by N.Kobayashi 2014/06/24
//           Y.Ebata
//
using UnityEngine;
using System.Collections.Generic;
using Esa;

namespace UnityChan
{
    public class SpringManager : MonoBehaviour
    {
        //Kobayashi
        // DynamicRatio is paramater for activated level of dynamic animation 
        public float dynamicRatio = 1.0f;

        //Ebata
        public float stiffnessForce;
        public AnimationCurve stiffnessCurve;
        public float dragForce;
        public AnimationCurve dragCurve;
        public List<SpringBone> springBones;

        //void Start()
        //{
        //    UpdateParameters();
        //}
        [Button]
        public void GenSpringBones()
        {
            var ts = transform.GetTransforms().RemoveLast();
            springBones = new List<SpringBone>();
            for (int i = 0; i < ts.Count - 1; i++)
            {
                var sb = ts[i].AddComponent<SpringBone>();
                springBones.Add(sb);
                sb.child = ts[i + 1];
                sb.colliders = new SpringCollider[0];
            }
        }
        [Button]
        public void StartSpring()
        {
            var mgrs = FindObjectsOfType<SpringManager>();
            var wind = FindObjectOfType<RandomWind>();
            if (wind != null) wind.StartWind();
            foreach (var mgr in mgrs)
            {
                foreach (var bone in mgr.springBones)
                {
                    bone.Init();
                    mgr.dynamicRatio = 1;
                }
            }
        }
#if UNITY_EDITOR
        void Update()
        {
            dynamicRatio = Mathf.Clamp01(dynamicRatio);
            UpdateParameters();
        }
#endif
        private void LateUpdate()
        {
            //Kobayashi
            if (dynamicRatio != 0.0f)
            {
                for (int i = 0; i < springBones.Count; i++)
                {
                    if (dynamicRatio > springBones[i].threshold)
                    {
                        springBones[i].UpdateSpring();
                    }
                }
            }
        }

        public void UpdateParameters()
        {
            //dynamicRatio = HairGen.I.springMgr.dynamicRatio;
            //dragForce = HairGen.I.springMgr.dragForce;
            //dragCurve = HairGen.I.springMgr.dragCurve;
            //stiffnessForce = HairGen.I.springMgr.stiffnessForce;
            //stiffnessCurve = HairGen.I.springMgr.stiffnessCurve; // wjk

            UpdateParameter("stiffnessForce", stiffnessForce, stiffnessCurve);
            UpdateParameter("dragForce", dragForce, dragCurve);
        }

        private void UpdateParameter(string fieldName, float baseValue, AnimationCurve curve)
        {
#if UNITY_EDITOR
            var start = curve.keys[0].time;
            var end = curve.keys[curve.length - 1].time;
            //var step	= (end - start) / (springBones.Length - 1);

            var prop = springBones[0].GetType().GetField(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            for (int i = 0; i < springBones.Count; i++)
            {
                //Kobayashi
                if (!springBones[i].isUseEachBoneForceSettings)
                {
                    var scale = curve.Evaluate(start + (end - start) * i / (springBones.Count - 1));
                    prop.SetValue(springBones[i], baseValue * scale);
                }
            }
#endif
        }
    }
}