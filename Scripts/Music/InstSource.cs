using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using System;
    public class InstSource : MonoBehaviour
    {
        public string path { get { _path = Application.streamingAssetsPath + folder + clipName; return _path; } }
        public string _path;
        public string folder = @"\Wav\Sound\Inst\Piano_1\";
        public string clipName = "a1";
        public string prefix = ".wav";
        [Button]
        void ViewPath() { _path = path; }
        public PitchRange rng;
        public float fadeDuration; // 淡出需要时间（秒）
        public AnimationCurve fadeCurve;
        public List<Source> srcs;
        void Start()
        {
            GenSrcs();
            LoadAllClip();
        }
        [Button]
        void GenSrcs()
        {
            srcs = new List<Source>();
            transform.ClearChildren();
            // 获取音域范围，生成一一对应的AudioSource组件
            var scl = rng.Scales;
            if (scl <= 0) return;
            if (scl == 1)
            {
                CreateRange(rng);
            }
            else
            {
                var rngMin = new PitchRange(rng.min, rng.min.sclMax);
                CreateRange(rngMin);
                for (var i = rng.min.scale + 1; i < rng.max.scale - 1; i++)
                {
                    var min = new Note(i, Pitch.C);
                    var max = new Note(i, Pitch.B);
                    var rngMid = new PitchRange(min, max);
                    CreateRange(rngMid);
                }
                var rngMax = new PitchRange(rng.max.sclMin, rng.max);
                CreateRange(rngMax);
            }
        }
        void CreateRange(PitchRange rng)
        {
            var min = rng[0];
            var max = rng[1];
            for (Pitch i = min.pitch; i < max.pitch; i++)
            {
                N(new Note(min.scale, i));
            }
        }
        void N(Note n)
        {
            var go = new GameObject(n.ToString2());
            go.SetParent(gameObject);
            go.AddComponent<AudioSource>();
            var src = go.AddComponent<Source>();
            srcs.Add(src);
            src.pitch = n;
            src.inst = this;
            //src.fadeCurve = fadeCurve;
            //src.fadeDuration = fadeDuration;
        }
        // Update is called once per frame
        void Update()
        {

        }
        public void Play(int i)
        {
            srcs[i].Play();
        }
        public void Play(Note n)
        {
            foreach (var src in srcs)
            {
                if (src.pitch.Equals(n))
                {
                    src.Play();
                    return;
                }
            }
            print("Play Note: " + n.ToString2() + " not found.");
        }
        [Button]
        public void LoadAllClip()
        {
            foreach (var src in srcs)
            {
                clipName = src.pitch.ToString2() + prefix;
                StartCoroutine(LoadClip(src, path));
            }
        }
        IEnumerator LoadClip(Source src, string path)
        {
            WWW w = new WWW(path);
            while (!w.isDone)
            {
                yield return 0;
            }
            src.audioSource.clip = w.GetAudioClip();
        }
    }
}