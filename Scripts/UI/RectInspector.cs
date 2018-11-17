using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Esa.UI
{
    [ExecuteInEditMode]
    public class RectInspector : MonoBehaviour
    {
        public bool updateInEditor = true;
        [Header("Anchors")]
        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public Vector2 offsetMin;
        public Vector2 offsetMax;
        public Vector2 pivot;

        [Header("Position")]
        public Vector2 anchoredPosition;
        public Vector2 sizeDelta;

        [Header("Rect")]
        public Vector2 rectSize;
        public Rt rect;

        [Header("Mouse")]
        public bool mouseOver;

        private RectTransform rt;
        private void Reset()
        {
            rt = transform as RectTransform;
        }
        void Start()
        {
            rt = transform as RectTransform;
        }
        bool modify;
        void Update()
        {
            if (!updateInEditor) return;
            if (modify)
            {
                rt.anchorMin = anchorMin;
                rt.anchorMax = anchorMax;
                rt.pivot = pivot;
            }
            else
            {
                anchorMin = rt.anchorMin;
                anchorMax = rt.anchorMax;
                offsetMin = rt.offsetMin;
                offsetMax = rt.offsetMax;
                pivot = rt.pivot;
                anchoredPosition = rt.anchoredPosition;
                sizeDelta = rt.sizeDelta;

                rectSize = rt.rect.size;
                rect = rt.GetRt();
                mouseOver = rect.Contains(UI.mousePosRef);
            }
        }
    }
}