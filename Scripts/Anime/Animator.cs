using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Esa
{
    public class Animator : MonoBehaviour
    {
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
        public bool walk
        {
            set
            {
                if (fps == 60 && !value) // turn off
                {
                    playTime = 0;
                    fps = 0;
                }
                else if (fps == 0 && value) // turn on
                {
                    fps = 60;
                }
            }
        }
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
        void Start()
        {
            playTime = 0;
            LoadClipsInPath();
        }
        public void SetClip(string clipName)
        {
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
                    curve.ast = GetComponent<Avatar>().GetTransDOF(curve.ast.dof);
                }

                ClipTool.GetPairs(c.curves);
                ClipTool.GetFrameRange(c);

                clips.Add(c);
            }
            if (clips.Count > 0)
            {
                current = clips[0];
                Mirror(current);
                print(current.clipName + " length: " + current.frameRange.y);
            }
        }
        Clip Mirror(Clip clip)
        {
            var i = 40;
            mirror = true;
            PlayIdx(i);
            clip.AddEulerPosAllCurve(80);
            PlayIdx(0);
            clip.AddEulerPosAllCurve(120);
            clip.frameRange.y *= 2;
            mirror = false;
            return clip;
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
            if (i > current.frameRange.y) playTime = 0;
            foreach (var ast in GetComponent<Avatar>().data.asts)
            {
                if (ast.dof.bone == Bone.root) continue;
                var curveAst = current.GetCurve(ast);
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