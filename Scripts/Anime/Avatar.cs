using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    [ExecuteInEditMode]
    public class Avatar : MonoXmlSL<AvatarData>
    {
        public TransDOF this[Transform t]
        {
            get { return GetTransDOF(t); }
        }
        public TransDOF this[Bone bone]
        {
            get { return GetTransDOF(bone); }
        }
        public TransDOF this[DOF dof]
        {
            get { return this[dof.bone]; }
        }
        public Transform rig;
        public DOFMgr dofMgr { get { return GetComponent<DOFMgr>(); } }
        public Bone selectBone;

        public float drawLineLength = 0.03f;
        public bool depthTest = false;
        public bool drawLine = true;
        public Color boneColor;

        [Button]
        public void UpdateCoord()
        {
            int count = 0;
            //Start();
            foreach (var t in data.asts)
            {
                //t.Init();
                if (t != null || t.transform != null)
                {
                    t.UpdateCoord();
                    count++;
                }
            }
            Debug.Log("UpdateCoords " + count.ToString());
        }
        public void UpdateCoordSel()
        {
            var td = GetTransDOF(selectBone);
            if (td == null || td.transform == null) return;

            td.UpdateCoord();

            Debug.Log("updateCoord " + td.transform.name);
        }
        [Button("ChildrenSwap Y & Z (exclude self)")]
        public void ChildrenSwapYAndZ()
        {
            var td = GetTransDOF(selectBone);
            if (td == null || td.transform == null) return;
            foreach (var t in td.transform.GetComponentsInChildren<Transform>(true))
            {
                if (t == td.transform) continue;
                var transD = GetTransDOF(t);
                if (transD == null) continue;
                var up = transD.up;
                transD.up = transD.forward;
                transD.forward = up;
                transD.UpdateCoord();
                Debug.Log("swap " + transD.transform.name);
            }
        }
        public void Reset()
        {
            rig = transform;
            folder = "Avatar/";
            fileName = "General.xml";
            Load();
        }
        public TransDOF GetTransDOF(DOF dof)
        {
            return GetTransDOF(dof.bone);
        }
        public TransDOF GetTransDOF(Bone bone)
        {
            foreach (var t in data.asts)
            {
                if (t.dof.bone == bone)
                {
                    return t;
                }
            }
            return null;
        }
        public TransDOF GetTransDOF(Transform trans)
        {
            foreach (var t in data.asts)
            {
                if (t.transform == trans)
                {
                    return t;
                }
            }
            return null;
        }
        [Button]
        public void ClearTrans()
        {
            foreach (var t in data.asts)
            {
                t.transform = null;
            }
        }
        [Button]
        void Match()
        {
            Match(true);
        }
        [Button("Match(not Init)")]
        void MatchNotInit()
        {
            Match(false);
        }
        public void Match(bool init)
        {
            var _dic = new Dictionary<Bone, Transform>();
            _dic.Add(Bone.root, rig);
            //RigHuman.MatchBones(_dic, rig, ZHuman.HumanSkeletonMap);
            RigHuman.MatchBones(_dic, rig, RigHuman.CreateHumanMap());
            foreach (var item in _dic)
            {
                var ast = GetTransDOF(item.Key);
                if (ast != null)
                {
                    ast.transform = item.Value;
                    Debug.Log("match: " + ast.transform.name);
                }
            }
            if (init) Start();
        }
        public void LoadFromDOFMgr()
        {
            foreach (var ast in data.asts)
            {
                var dof = dofMgr.GetDOF(ast.dof.bone);
                ast.dof = dof;
            }
        }
        private void Start()
        {
            foreach (var t in data.asts)
            {
                if (t.transform != null) t.Init(); // 获取坐标轴的初始值
            }
        }
        public void Update()
        {
#if UNITY_EDITOR
            if (drawLine)
                data.DrawLines(boneColor, drawLineLength, depthTest);
            if (Application.isPlaying)
#endif
                data.UpdateTrans();
        }
        [Button]
        public void SetOrigin()
        {
            foreach (var t in data.asts)
            {
                if (t != null && t.transform != null)
                    t.coord.origin = t.transform.localRotation;
            }
        }
    }
}