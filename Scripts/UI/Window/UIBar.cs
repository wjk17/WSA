using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Esa.UI_
{
    using System;
    [Serializable]
    public class ProgBar
    {
        public float t; // 0~1
        public Rect curr;
        public Rect bg;
        public Color color;
        public Vector2 size
        {
            get { return bg.size; }
        }
        public Vector2 pos
        {
            set { SetPos(value); }
            get { return curr.pos; }
        }
        public void SetPos(Vector2 pos)
        {
            var w = curr.size.x;
            curr.size.x = bg.size.x;
            curr.SetPos(pos, Vectors.half2d);
            bg.SetPos(pos, Vectors.half2d);
            curr.size.x = w;
        }
        public ProgBar(Vector2 pos, Vector2 size, Color color)
        {
            curr = new Rect(pos, size);
            curr.SetPivot(Vector2.zero);
            bg = curr.Clone();
            this.color = color;
        }
        public void Update(float t)
        {
            t = Mathf.Clamp01(t);
            this.t = t;
            curr.size.x = t * bg.size.x;
            GLUI.DrawQuad(Color.white, bg.ToCW().ToArray());
            GLUI.DrawQuad(color, curr.ToCW().ToArray());
        }
    }
}
namespace Esa
{
    public class UIBar : MonoBehaviour
    {
        public float t; // 0~1
        public RectTransform curr;
        RectTransform rt;
        void Start()
        {
            rt = transform as RectTransform;
            curr = transform.GetChild(0) as RectTransform;
        }
        void Update()
        {
            t = Mathf.Clamp01(t);
            curr.offsetMax = curr.offsetMax.SetX(-(1 - t) * rt.rect.size.x);
        }
    }
}