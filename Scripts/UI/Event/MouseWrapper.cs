using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace Esa
{
    public class MouseEventWrapper : MonoBehaviour
    {
        public Action onMouseOver;
        public Action onMouseEnter;
        public Action onMouseExit;
        public Action onMouseDown;
        public Action onMouseUp;
        public Action onMouseDrag;
        public bool over;
        public void CreateBox2D()
        {
            var box = gameObject.AddComponent<BoxCollider2D>();
            var rt = (transform as RectTransform);
            box.size = rt.rect.size;
            box.offset = Vector2.Scale(rt.rect.size, Vector2.one * 0.5f - rt.pivot);
        }
        private void OnMouseOver()
        {
            if (onMouseOver != null) onMouseOver();
        }
        private void OnMouseEnter()
        {
            over = true;
            if (onMouseEnter != null) onMouseEnter(); 
        }
        private void OnMouseExit()
        {
            over = false;
            if (onMouseExit != null) onMouseExit();
        }
        private void OnMouseDown()
        {
            if (onMouseDown != null) onMouseDown();
        }
        private void OnMouseUp()
        {
            if (onMouseUp != null) onMouseUp();
        }
        private void OnMouseDrag()
        {
            if (onMouseDrag != null) onMouseDrag();
        }
    }
}