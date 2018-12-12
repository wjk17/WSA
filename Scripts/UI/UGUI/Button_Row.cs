using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Esa
{
    public class Button_Row : MonoBehaviour
    {
        public string[] buttons;
        public float xOs;
        public Color colorNormal;
        public Color colorOver;
        public Color colorDown;
        public Action<int> onClick;
        [Button]
        void Start()
        {
            var p = transform.GetChild(0);
            int i = 0;
            xOs += (p as RectTransform).rect.size.x;
            foreach (var btn in buttons)
            {
                var t = Instantiate(p, transform, true);
                (t as RectTransform).anchoredPosition += Vector2.right * xOs * i;
                t.GetComponentInChildren<Text>().text = btn;
                var img = t.GetComponent<Image>();
                var n = i++;
                var mw = t.AddComponent<MouseEventWrapper>();
                img.color = colorNormal;
                mw.onMouseDown = () => { img.color = colorDown; ItemClick(n); };
                mw.onMouseEnter = () => { img.color = colorOver; };
                mw.onMouseExit = () => { img.color = colorNormal; };
                mw.onMouseUp = () => { img.color = mw.over ? colorOver : colorNormal; };
                mw.CreateBox2D();
            }
            p.gameObject.SetActive(false);
        }
        void ItemClick(int idx)
        {
            onClick(idx);
        }
    }
}