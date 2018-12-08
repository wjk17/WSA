using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa.UI
{
    public static class OnMove_Tool
    {
        public static List<OnMove> inss = new List<OnMove>();
        public static void CheckMove(this MonoBehaviour mono, Action onMove)
        {
            var ins = Find(inss, mono);
            if (ins == null)
            {
                ins = inss.Add_(new OnMove(mono.transform as RectTransform, onMove));
            }
            else ins.onResize = onMove;
            ins.CheckAndSolve();
        }
        public static OnMove Find(List<OnMove> list, MonoBehaviour item)
        {
            foreach (var t in list)
            {
                if (t.rt.transform == item.transform) return t;
            }
            return null;
        }
    }
    [Serializable]
    public class OnMove
    {
        public RectTransform rt;
        public Vector2 prevPos;
        public Action onResize;
        public OnMove(RectTransform rt, Action onPosChange)
        {
            this.onResize = onPosChange;
            this.rt = rt;
            prevPos = rt.anchoredPosition;
        }
        public void CheckAndSolve()
        {
            if (rt.anchoredPosition != prevPos)
            {
                if (onResize != null) onResize();
            }
            prevPos = rt.anchoredPosition;
        }
    }
}