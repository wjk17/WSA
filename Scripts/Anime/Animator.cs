using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Esa
{
    public class Animator : MonoBehaviour
    {
        public bool finished
        {
            get
            {
                return playTime * fps >= current.frameRange.y;
            }
        }
        public float nTime
        {
            get
            {
                return playTime * fps / current.frameRange.y;
            }
        }
        public static Clip current
        {
            get
            {
                return _current;
            }
            set { _current = value; }
        }
        private static Clip _current;
        public bool play = true;
        public string _path;
        public string path
        {
            get
            {
                _path = Path.Combine(Application.streamingAssetsPath, folder);
                return _path;
            }
        }
        public string folder = "Clip/";

        public List<Clip> clips;

        public float playTime;
        public float timeFactor = 1;

        public int fps = 60;
        public bool mirror;
        public bool loop = true;

        public string clipName;

        void Start()
        {
            playTime = 0;
            LoadClipsInPath();
        }
        public void SetClip(string clipName)
        {
            if (current != null && clipName == current.clipName) return;
            playTime = 0;
            foreach (var clip in clips)
            {
                if (clip.clipName == clipName)
                {
                    current = clip;
                }
            }
        }
        private void LoadClipsInPath()
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fis = dir.GetFiles("*.clip", SearchOption.TopDirectoryOnly);
            clips = new List<Clip>();
            foreach (var fi in fis)
            {
                var c = Serializer.XMLDeSerialize<Clip>(fi.FullName);
                c.clipName = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

                foreach (var curve in c.curves)
                {
                    if (curve.ast == null) continue;
                    curve.ast = GetComponent<Avatar>().GetTransDOF(curve.ast.dof);
                }

                ClipTool.GetPairs(c.curves);
                ClipTool.GetFrameRange(c);

                clips.Add(c);
            }
            if (clips.Count > 0)
            {
                SetClip(clipName);
                if (SYS.debugAnime) print(current.clipName + " length: " + current.frameRange.y);
            }
        }
        Clip Mirror(Clip clip)
        {
            mirror = true;
            clip.loop = true;
            PlayIdx(0);
            clip.AddEulerPosAllCurve(80);
            PlayIdx(40);
            clip.AddEulerPosAllCurve(120);
            mirror = false;
            PlayIdx(0);
            clip.AddEulerPosAllCurve(160);

            ScaleClip(clip, 0.5f);

            ClipTool.GetFrameRange(clip);
            return clip;
        }
        public void ScaleClip(Clip clip, float factor)
        {
            foreach (var oc in clip.curves)
            {
                foreach (var curve in oc.curves)
                {
                    foreach (var key in curve.keys)
                    {
                        key.time *= factor;
                    }
                }
            }
        }
        private void PlayEveryFrame()
        {
            if (play)
            {
                playTime += Time.deltaTime * timeFactor;
                PlayTime(playTime);
            }
        }
        void PlayTime(float time)
        {
            PlayIdx(Mathf.RoundToInt(time * fps));
        }
        void PlayIdx(int i)
        {
            if (i > current.frameRange.y)
            {
                if (current.loop)
                    i = (int)Mathf.Repeat(i, current.frameRange.y);
                else i = current.frameRange.y;
            }
            foreach (var ast in GetComponent<Avatar>().data.asts)
            {
                if (ast.dof.bone == Bone.root) continue;
                var curveAst = current.GetCurve(ast); // 从clip里找到对应的曲线
                if (curveAst == null)
                {
                    //if (debug) print("没找到: " + Enum.GetName(typeof(Bone), ast.dof.bone));
                    continue;
                }
                if (mirror)
                {
                    var pair = curveAst.pair;
                    if (ast.transform != null)
                    {
                        if (pair == null)
                            ast.euler = curveAst.Rot(i).MirrorX();
                        else
                            ast.euler = pair.Rot(i).MirrorEulerX();
                    }
                }
                else
                {
                    ast.euler = curveAst.Rot(i);
                    if (curveAst.pos.x.Count > 0)
                    {
                        ast.pos = curveAst.Pos(i);
                    }
                }
            }
        }
        void Update()
        {
            PlayEveryFrame();
        }
    }
}