using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class Source : MonoBehaviour
    {
        public AudioSource audioSource;
        public float tFade; // 淡出进度
        //public float fadeDuration; // 淡出需要时间（秒）
        //public AnimationCurve fadeCurve;
        public bool hold; // 延音
        public Note pitch;
        public InstSource inst;
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void Reset()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        void Update()
        {
            if (hold || tFade > 1.2) return;

            var delta = Time.deltaTime / inst.fadeDuration;
            tFade += delta;
            audioSource.volume = inst.fadeCurve.Evaluate(tFade);
        }
        [Button]
        public void Play()
        {
            tFade = 0;
            audioSource.volume = 1;
            audioSource.Play();
        }
    }
}