using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa._UI
{
    /// <summary>
    /// mono类里this.CheckResize，需每帧调用，第一次调用会在列表里插入实例。
    /// 大小或位置改变时会触发 onResize 事件。
    /// </summary>
    public static class OnResize_Tool
    {
        public static List<OnResize> inss = new List<OnResize>();
        public static bool CheckResize(this MonoBehaviour mono, Action onResize)
        {
            // 插入到静态列表
            var ins = Find(inss, mono);
            if (ins == null)
            {
                ins = inss.Add_(new OnResize(mono.transform as RectTransform, onResize));
            }
            else ins.onResize = onResize;
            return ins.CheckAndSolve();
        }
        public static OnResize Find(List<OnResize> list, MonoBehaviour item)
        {
            foreach (var t in list)
            {
                if (t.rt.transform == item.transform) return t;
            }
            return null;
        }
    }

    [Serializable]
    public class OnResize
    {
        public RectTransform rt;
        public Vector2 prevPos;
        public Vector2 prevSize;
        public Action onResize;
        public OnResize(RectTransform rt, Action onResize)
        {
            this.onResize = onResize;
            this.rt = rt;
            prevPos = rt.anchoredPosition;
            prevSize = rt.sizeDelta;
        }
        public bool CheckAndSolve()
        {
            if (rt.anchoredPosition != prevPos || rt.sizeDelta != prevSize)
            {
                if (onResize != null) { onResize(); return true; }
            }
            prevPos = rt.anchoredPosition;
            prevSize = rt.sizeDelta;
            return false;
        }
    }
}