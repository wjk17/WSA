﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa.UI_
{
    public class Button_OnClick_Key : MonoBehaviour
    {
        public KeyCode keyCode;
        void Update()
        {
            if (Events.KeyDown(keyCode))
                GetComponent<Button>().onClick.Invoke();
        }
    }
}