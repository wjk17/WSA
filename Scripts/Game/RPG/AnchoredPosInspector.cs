using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    [ExecuteInEditMode]
    public class AnchoredPosInspector : MonoBehaviour
    {
        public Vector2 anchoredPos;
        public Vector2 sizeDelta;
        public Vector2 offsetMin;
        public Vector2 offsetMax;
        [Header("Corner")]
        public Vector2 _cornerRT;
        public Vector2 _cornerLB;
        public Vector2 _center;
        [Header("Esa")]
        public Vector2 _anchoredPos;
        public Vector2 _sizeDelta;
        public Vector2 _offsetMin;
        public Vector2 _offsetMax;
        RectTrans rect;
        RectTransform RT;
        public bool update;
        [Button]
        void GetRect()
        {
            RT = transform as RectTransform;
            rect = new RectTrans(this);
            Update();
        }
        void Update()
        {
            if (RT == null) return;
            if (update) rect = new RectTrans(this);
            rect.pivot = RT.pivot;
            rect.anchorMin = RT.anchorMin;
            rect.anchorMax = RT.anchorMax;

            _cornerRT = rect.cornerRT;
            _cornerLB = rect.cornerLB;
            _center = rect.center;

            offsetMax = RT.offsetMax;
            offsetMin = RT.offsetMin;
            anchoredPos = RT.anchoredPosition;

            _offsetMax = rect.offsetMax;
            _offsetMin = rect.offsetMin;
            _anchoredPos = rect.anchoredPos;
        }
    }
}