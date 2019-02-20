using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class Source : MonoBehaviour
    {
        public AudioSource audioSource;
        public float tFade; // 淡出进度
        public float tAdjust;

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
            if (tFade > inst.fadeDuration * 1.2f &&
                tAdjust > inst.adjustDuration * 1.2f) return;

            tAdjust += Time.deltaTime;

            audioSource.volume = inst.adjustCurve.Evaluate(tAdjust / inst.adjustDuration);

            if (!hold) tFade += Time.deltaTime;

            audioSource.volume *= inst.fadeCurve.Evaluate(tFade / inst.fadeDuration);
        }
        [Button]
        public void Play()
        {
            tFade = 0;
            tAdjust = 0;
            audioSource.volume = 1;
            audioSource.Play();
        }
    }
}