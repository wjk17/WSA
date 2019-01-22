using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    public class UIClip : Singleton<UIClip>
    {
        public Clip clip
        {
            get
            {
                if (_clip == null || _clip.curves.Count == 0) I.Load();
                return _clip;
            }
            set { _clip = value; }
        }
        [SerializeField]
        private Clip _clip;
        [Header("ReadOnly")]
        public string path;
        public string folder = "Clip/";
        public string clipName = "Default";
        public bool debug = false;
        // 拖动时间轴绿色线（当前帧）时会更新所有曲线
        public void UpdateAllCurve()
        {
            var idx = UITimeLine.I.frameIdx;
            if (clip.curves.Count <= 0) return;
            foreach (var curve in clip.curves)
            {
                if (curve.ast != null)
                {
                    if (debug) print("update: " + curve.ast.transform.name);
                    curve.ast.euler = curve.Rot(idx);
                    if (UITranslator.I.update.isOn)
                    {
                        if (curve.poss[0].keys != null && curve.poss[0].keys.Count > 0)//有位置曲线才更新位置
                        {
                            var pos = curve.Pos(idx);
                            var os = curve.ast.coord.originPos;
                            curve.ast.transform.localPosition = os + pos;
                        }
                    }
                    if (UIDOFEditor.I.ast == null || UIDOFEditor.I.ast.dof == null)
                    {
                        throw null;
                    }
                    // 把值实时显示到两个编辑器
                    if (curve.ast.dof.bone == UIDOFEditor.I.ast.dof.bone)
                    {
                        UIDOFEditor.I.UpdateValueDisplay();
                        UITranslator.I.UpdateValueDisplay();
                    }
                }
            }
            UIPlayer.I.Mirror();
        }
        [Button]
        void Load_()
        {
            Load(name);
        }
        public System.Action onLoadClip;
        public bool Load(string clipName)//不存在文件则返回false
        {
            path = UIClipList.I.clipPath + clipName + ".clip";
            if (System.IO.File.Exists(path))
            {
                _clip = Serializer.XMLDeSerialize<Clip>(path);
                _clip.clipName = clipName;
                foreach (var curve in _clip.curves)
                {
                    var trans = UIDOFEditor.I.avatar.transform.Search(curve.name);
                    curve.ast = UIDOFEditor.I.avatar.GetTransDOF(trans);
                }
                ClipTool.GetPairs(_clip.curves);
                ClipTool.GetFrameRange(_clip);

                PlayerPrefs.SetString("LastOpenClipName", clipName);
                PlayerPrefs.Save();
                if (onLoadClip != null) onLoadClip();
                return true;
            }
            else
            {
                Debug.Log("路径: " + path + " 不存在。");
                return false;
            }
        }

        public bool Load()
        {
            path = UIClipList.I.clipPath + clipName + ".clip";
            if (System.IO.File.Exists(path))
            {
                _clip = Serializer.XMLDeSerialize<Clip>(path);
                _clip.clipName = clipName;
                foreach (var curve in _clip.curves)
                {
                    //curve.trans = UIDOFEditor.I.avv atar.transform.Search(curve.name);
                    var trans = UIDOFEditor.I.avatar.transform.Search(curve.name);
                    curve.ast = UIDOFEditor.I.avatar.GetTransDOF(trans);
                }
                ClipTool.GetPairs(_clip.curves);
                ClipTool.GetFrameRange(_clip);
                //PlayerPrefs.SetString("LastOpenClipName", clipName);
                //PlayerPrefs.Save();
                return true;
            }
            else  // 不存在clip文件则新建一个
            {
                _clip = new Clip(clipName);
                foreach (var ast in UIDOFEditor.I.avatar.data.asts)
                {
                    _clip.AddCurve(ast);
                }
                ClipTool.GetPairs(_clip.curves);
                ClipTool.GetFrameRange(_clip);
                //PlayerPrefs.SetString("LastOpenClipName", clipName);
                //PlayerPrefs.Save();
                return false;
            }
        }
        [Button]
        void NewClip()
        {
            New(name);
        }
        public void New(string clipName)
        {
            var c = new Clip(clipName);
            foreach (var ast in UIDOFEditor.I.avatar.data.asts)
            {
                c.AddCurve(ast);
            }
            ClipTool.GetPairs(c.curves);
            ClipTool.GetFrameRange(c);
            PlayerPrefs.SetString("LastOpenClipName", clipName);
            PlayerPrefs.Save();

            path = UIClipList.I.clipPath + clipName + ".clip";
            clip = Serializer.XMLSerialize(c, path);
        }
        public void Save(string clipName)
        {
            path = UIClipList.I.clipPath + clipName + ".clip";
            Serializer.XMLSerialize(clip, path);
        }
        public void Save()
        {
            path = UIClipList.I.clipPath + clip.clipName + ".clip";
            Serializer.XMLSerialize(clip, path);
        }
    }
}