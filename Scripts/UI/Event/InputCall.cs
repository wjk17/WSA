using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa.UI_
{
    public static class InputCall_Tool
    {
        // 自动使用mono对象的名字和rectTransform
        public static InputCall AddInput(this MonoBehaviour mono)
        {
            return UI.I.inputs.Add_(new InputCall(mono, null, 0));
        }
        public static InputCall AddInput(this MonoBehaviour mono, int order)
        {
            return UI.I.inputs.Add_(new InputCall(mono, null, order));
        }
        // checkOver 是否检测鼠标是否悬停（mono使用null）
        public static InputCall AddInput(this MonoBehaviour mono, Action updateFunc, int order, bool checkOver = true)
        {
            GLHandler h = new GLHandler(mono.gameObject);
            h.order = order;
            h.name = mono.name;
            UI.I.glHandlers.Add(h);
            UI.I.imHandlers.Add(new IMHandler(mono.gameObject));
            return UI.I.inputs.Add_(new InputCall(mono, updateFunc, order, checkOver));
        }
    }
    [Serializable]
    public class InputCall
    {
        public InputCall() { }
        /// <summary>
        /// 降序
        /// </summary>
        public InputCall(MonoBehaviour mono, Action getInput, int order = 0)
        {
            this.mono = mono;
            this.gameObject = mono.gameObject;
            this.RT = mono.transform as RectTransform;
            this.name = mono.name;
            this.getInput = getInput;
            this.order = order;
        }
        public InputCall(MonoBehaviour mono, Action getInput, int order = 0, bool checkOver = true)
        {
            this.mono = mono;
            this.gameObject = mono.gameObject;
            this.RT = mono.transform as RectTransform;
            this.name = mono.name;
            this.getInput = getInput;
            this.order = order;
            this.checkOver = checkOver;
        }
        public InputCall(string name, Action getInput, int order = 0)
        {
            this.name = name;
            this.getInput = getInput;
            this.order = order;
        }
        // for inspector
        public bool enabled;

        public bool on = true;
        public GameObject gameObject;
        public RectTransform RT;
        public Rect rt;
        public MonoBehaviour mono;
        public bool mouseOver;
        public bool checkOver;
        public string name;
        public Action getInput;
        public int order;
    }
}