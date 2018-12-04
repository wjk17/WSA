using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa.UI
{
    public static class InputCallBack_Tool
    {
        // 自动使用mono对象的名字和rectTransform
        public static InputCallBack AddInputCB(this MonoBehaviour mono)
        {
            return UI.I.inputCallBacks.Add_R(new InputCallBack(mono, null, 0));
        }
        public static InputCallBack AddInputCB(this MonoBehaviour mono, int order)
        {
            return UI.I.inputCallBacks.Add_R(new InputCallBack(mono, null, order));
        }
        public static InputCallBack AddInputCB(this MonoBehaviour mono, Action updateFunc, int order)
        {
            return UI.I.inputCallBacks.Add_R(new InputCallBack(mono, updateFunc, order));
        }
        // 自定义名字，mono为null时rectTransform为null
        public static InputCallBack AddInputCB(this MonoBehaviour mono, string name, Action updateFunc, int order)
        {
            return UI.I.inputCallBacks.Add_R(new InputCallBack(name, updateFunc, order));
        }
    }
    [Serializable]
    public class InputCallBack
    {
        public InputCallBack() { }
        /// <summary>
        /// 降序
        /// </summary>
        public InputCallBack(MonoBehaviour mono, Action getInput, int order = 0)
        {
            this.mono = mono;
            this.gameObject = mono.gameObject;
            this.RT = mono.transform as RectTransform;
            this.name = mono.name;
            this.getInput = getInput;
            this.order = order;
        }
        public InputCallBack(string name, Action getInput, int order = 0)
        {
            this.name = name;
            this.getInput = getInput;
            this.order = order;
        }
        public GameObject gameObject;
        public RectTransform RT;
        public Rt rt;
        public MonoBehaviour mono;
        public bool mouseOver;
        public string name;
        public Action getInput;
        public int order;
    }
}