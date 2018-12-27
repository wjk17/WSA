using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa.UI
{
    public partial class UIDOFEditor : Singleton<UIDOFEditor>
    {
        public Bone bone = Bone.chest; // 给出个初始值

        public Vector3 lockPos1;
        public Vector3 lockPos2;

        public DOF dof;
        public DOFMgr dofSet
        {
            get
            {
                if (_dofSet == null)
                    _dofSet = FindObjectOfType<DOFMgr>(); return _dofSet;
            }
        }
        DOFMgr _dofSet;
        public Avatar avatar
        {
            get
            {
                if (_avatar == null)
                    _avatar = FindObjectOfType<Avatar>(); return _avatar;
            }
        }
        Avatar _avatar;

        public Transform target;
        public Transform end;
        public int iter;

        public TransDOF ast;
        public TransDOF astIK;

        public float alpha; // 逼近的步长
        public float theta0;
        public float theta1;

        public List<Bone> joints;
        public List<Bone> joints2;
        public int jointIterCount = 10;
        public int axisIterCount = 20;

        public Transform exBone;
        public frame frameClipBoard;

        public Vector3 exBone2LeftHand;
        public Vector3 exBone2RightHand;

        [HideInInspector]
        public UIDOFEditor_Fields f;

        public int CB_Order = 1;
        public bool debug = false;
        void Start()
        {
            this.AddInput(GetInput, CB_Order, false);

            foreach (var curve in UIClip.I.clip.curves)
            {
                curve.ast.coord.originPos = curve.ast.transform.localPosition;
            }

            dofSet.Load(); // 强制从文件里读取
                           //avatar.LoadFromDOFMgr(); // 从内存里读取
            dof = dofSet[bone];
            ast = avatar[bone];

            InitUI();

            UITimeLine.I.onFrameIdxChanged = OnFrameIdxChanged;

            UpdateDOF();
            var astOther = avatar[Bone.other];
            if (astOther.transform != null) exBone = astOther.transform;
        }
        void OnFrameIdxChanged(int frameIdx)
        {
            foreach (var oc in UIClip.I.clip.curves)
            {
                if (!oc.Empty())
                    oc.Update(UITimeLine.I.frameIdx);
            }
            UpdateDOF();
            //curve
        }
        public void InsertKeyToAllCurves()
        {
            UITimeLine.I.InsertKey();
        }
        private void OnFileNameChanged(string v)
        {

        }
        void NewClip()
        {
            UIClip.I.New(f.inputFileName.text);
            f.labelFileName.text = f.inputFileName.text;
            UIClipList.I.GetClipNamesInPath();
            UIClip.I.UpdateAllCurve();
            print("新建 " + f.labelFileName.text);
        }
        void LoadClip()
        {
            UIClip.I.Load(f.inputFileName.text);
            f.labelFileName.text = f.inputFileName.text;
            UIClip.I.UpdateAllCurve();
            UICurve.Curve = UIClip.I.clip.GetCurve(ast);

            print("读取 " + f.labelFileName.text);
            print("读取曲线 " + UICurve.Curve.name);
        }
        void SaveClip()
        {
            UIClip.I.Save(f.inputFileName.text);
            UIClipList.I.GetClipNamesInPath();
            print("保存 " + f.inputFileName.text);
        }
        void SaveAvatarSetting()
        {
            // 先修改DOF集，再引用到当前化身（Avatar）。不同化身，如不同化身（但骨架形状类似）的人物，生物可能使用相同的DOF集。
            dofSet.Save();
            avatar.LoadFromDOFMgr();
            avatar.Save();
        }
        void GetInput()
        {
            var hover = UI.MouseOver(UICurve.I, UICamera.I);
            // Frame
            if (hover && Input.GetKeyDown(KeyCode.C))
            {
                CopyFrame();
            }
            else if (hover && Input.GetKeyDown(KeyCode.V))
            {
                PasteFrame();
            }
            // IK
            if (f.toggleIK.isOn || f.toggleIKSingle.isOn)
            {
                IKSolve(joints.ToArray());
            }
            else if (f.toggleLockOneSide.isOn || f.toggleLockMirror.isOn)
            {
                end = avatar[joints[0]].transform;
                IKSolve(lockPos1, joints.ToArray());
                if (f.toggleLockMirror.isOn)
                {
                    end = avatar[joints2[0]].transform;
                    IKSolve(lockPos2, joints2.ToArray());
                }
            }
            else if (f.toggleWeaponIK.isOn) ExBoneIK();
        }
    }
}