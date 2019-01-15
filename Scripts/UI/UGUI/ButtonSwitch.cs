﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
namespace Esa._UI
{
    public class ButtonSwitch : MonoBehaviour
    {
        public Button[] buttons;
        public int index = 0;
        [Button]
        public void Switch()
        {
            if (buttons.Length <= 0) return;
            index++;
            if (index > buttons.Length - 1) index = 0;

            for (int i = 0; i < buttons.Length; i++)
            {
                if (i == index)
                {
                    buttons[i].gameObject.SetActive(true);
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }
        private void Reset()
        {
            buttons = GetComponentsInChildren<UnityEngine.UI.Button>(true);

        }
        public void Reg(params UnityAction[] callbacks)
        {
            if (buttons.Length < callbacks.Length)
            {
                Debug.Log("事件数超过按钮数");
                return;
            }
            // 注册点击事件        
            for (int i = 0; i < callbacks.Length; i++)
            {
                buttons[i].onClick.AddListener(callbacks[i]);
                buttons[i].onClick.AddListener(Switch);
            }
        }
        private void Awake()
        {
            if (buttons == null) Reset();
            index = -1;
            Switch();
        }
    }
}