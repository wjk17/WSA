using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
#if UNITY_EDITOR
    using UnityEditor;
#endif
    [ExecuteInEditMode]
    public class FollowPosition : MonoBehaviour
    {
        [Range(0, 1)]
        public float weight = 1f;
        public bool lateUpdate;
        public bool update = true;
        public bool fixedUpdate;
        public bool editorUpdate;
        public Bool2 x;
        public Bool2 y;
        public Bool2 z;
        Vector3 originPos;
        public Transform target;
        [Header("偏移值")]
        public bool world = false;
        public Vector3 offset;
        public bool getOffsetOnAwake = true;
        public bool getOffsetOnStart = true;
        public bool disableWhenSelect = true;
        void Reset()
        {
            x.bool2Label = y.bool2Label = z.bool2Label = "翻转";
        }
        private void Awake()
        {
            if (getOffsetOnAwake) GetOffset();
        }
        private void Start()
        {
            if (getOffsetOnStart) GetOffset();
        }
        [Button]
        void GetOffset()
        {
            originPos = world ? transform.position : transform.localPosition;
            offset = world ? transform.position - target.position
               : transform.localPosition - target.localPosition;
        }
        private void LateUpdate()
        {
            if (lateUpdate) DoUpdate();
        }
        private void Update()
        {
#if UNITY_EDITOR
            if (disableWhenSelect && Selection.activeGameObject == gameObject) return;
            if (!Application.isPlaying && editorUpdate) DoUpdate();
            else if (Application.isPlaying && update) DoUpdate();
#else
        if (update) DoUpdate();
#endif
        }
        private void FixedUpdate()
        {
            if (fixedUpdate) DoUpdate();
        }
        [Button]
        public void DoUpdate()
        {
            var posA = originPos;
            var posB = (world ? target.position : target.localPosition) + offset;
            float b;
            if (x.bool1)
            {
                b = x.bool2 ? -posB.x : posB.x;
                transform.SetPosX(Mathf.Lerp(posA.x, b, weight), world);
            }
            if (y.bool1)
            {
                b = y.bool2 ? -posB.y : posB.y;
                transform.SetPosY(Mathf.Lerp(posA.y, b, weight), world);
            }
            if (z.bool1)
            {
                b = z.bool2 ? -posB.z : posB.z;
                transform.SetPosZ(Mathf.Lerp(posA.z, b, weight), world);
            }
        }
    }
}