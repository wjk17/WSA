using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa._UI
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
        public Vector2 absPosition;
        public Vector2 absPositionParent;

        [Header("Rect")]
        public Vector2 rectSize;
        public Vector2 rectPos;
        public Rect rect;

        [Header("Mouse")]
        public bool mouseOver;

        private RectTransform rt;
        bool modify;
        public float rectSideLength = 10f;

        public bool draw;
        private void Reset()
        {
            Start();
        }
        void Start()
        {
            rt = transform as RectTransform;
        }
        void Update()
        {
            if (!Application.isPlaying && !updateInEditor) return;

            if (draw)
            {
                this.BeginFrame();
                this.Draw();
            }
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
                rectPos = rt.rect.position;

                rect = new Rect(rt);
                absPosition = rect.pos;
                absPositionParent = UI.AbsRefPos(rt.parent);

                if (draw) UITool.DrawSquare(absPositionParent, rectSideLength, Color.blue);

                mouseOver = rect.Contains(UI.mousePosRef);
            }
        }
    }
}