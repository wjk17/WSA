﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Esa.UI_
{
    [ExecuteInEditMode]
    public class Button_Tab : MonoBehaviour
    {
        public string[] buttons;
        public List<GameObject> btns;
        public List<bool> clickable;
        public Vector2 offset = new Vector2(10, 0);
        public Vector2 factor = new Vector2(1, 0);
        public Color colorNormal;
        public Color colorOver;
        public Color colorDown;
        //public Color colorSelect;
        public Action<int> onClick;
        public bool updateInEditor;
        public bool updateImmd;
        public Transform prefab;

        public int idx = -1;
        private void Reset()
        {
            colorNormal = Color.grey;
            colorOver = Color.white;
            colorDown = Color.grey;
            prefab = transform.GetChild(0);
        }
        private void Update()
        {
            if ((updateInEditor && !Application.isPlaying) || updateImmd)
            {
                OnEnable();
            }
        }
        [Button]
        public void OnEnable()
        {
            //if (transform.childCount > 1) return; // 是否已经提前初始化
            prefab = transform.GetChild(0);
            foreach (var child in transform.GetChildsL1())
            {
                if (child != prefab)
                {
                    child.SetParent(null, false);// UI.Root;
                    ComTool.DestroyAuto(child.gameObject);
                }
            }
            prefab.gameObject.SetActive(true);
            var os = offset + (prefab as RectTransform).rect.size * factor;
            btns = new List<GameObject>();
            clickable = new List<bool>();
            int i = 0;
            foreach (var btn in buttons)
            {
                var t = Instantiate(prefab, transform, true);
                btns.Add(t.gameObject);
                clickable.Add(true);
                (t as RectTransform).anchoredPosition += os * i;
                t.GetComponentInChildren<Text>().text = btn;
                var img = t.GetComponent<Image>();
                var n = i++;
                var mw = t.AddComponent<MouseEventWrapper>();
                img.color = (i - 1) == idx ? colorDown : colorNormal;
                mw.onMouseDown = () =>
                {
                    foreach (var b in btns)
                    {
                        b.GetComponent<Image>().color = colorNormal;
                    }
                    idx = n; img.color = colorDown; ItemClick(n);
                };
                mw.onMouseEnter = () => { if (n != idx) img.color = colorOver; };
                mw.onMouseExit = () => { if (n != idx) img.color = colorNormal; };
                //mw.onMouseUp = () => { img.color = mw.over ? colorOver : colorNormal; };
                mw.CreateBox2D();
            }
            prefab.gameObject.SetActive(false);
        }
        void ItemClick(int idx)
        {
            if (onClick != null) onClick(idx);
        }
    }
}