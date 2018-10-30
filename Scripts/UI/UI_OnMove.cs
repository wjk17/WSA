using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class UI_OnMove_Tool
{
    public static List<UI_OnMove> inss = new List<UI_OnMove>();
    public static void CheckMove(this MonoBehaviour mono, Action onMove)
    {
        var ins = Find(inss, mono);
        if (ins == null)
        {
            ins = inss.Add_R(new UI_OnMove(mono.transform as RectTransform, onMove));
        }
        else ins.onResize = onMove;
        ins.CheckAndSolve();
    }
    public static UI_OnMove Find(List<UI_OnMove> list, MonoBehaviour item)
    {
        foreach (var t in list)
        {
            if (t.rt.transform == item.transform) return t;
        }
        return null;
    }
}
[Serializable]
public class UI_OnMove
{
    public RectTransform rt;
    public Vector2 prevPos;
    public Action onResize;
    public UI_OnMove(RectTransform rt, Action onPosChange)
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
/// <summary>
/// mono类里this.CheckResize，需每帧调用，第一次调用会在列表里插入实例。
/// 大小或位置改变时会触发 onResize 事件。
/// </summary>
public static class UI_OnResize_Tool
{
    public static List<UI_OnResize> inss = new List<UI_OnResize>();
    public static void CheckResize(this MonoBehaviour mono, Action onResize)
    {
        // 插入到静态列表
        var ins = Find(inss, mono);
        if (ins == null)
        {
            ins = inss.Add_R(new UI_OnResize(mono.transform as RectTransform, onResize));
        }
        else ins.onResize = onResize;
        ins.CheckAndSolve();
    }
    public static UI_OnResize Find(List<UI_OnResize> list, MonoBehaviour item)
    {
        foreach (var t in list)
        {
            if (t.rt.transform == item.transform) return t;
        }
        return null;
    }
}

[Serializable]
public class UI_OnResize
{
    public RectTransform rt;
    public Vector2 prevPos;
    public Vector2 prevSize;
    public Action onResize;
    public UI_OnResize(RectTransform rt, Action onResize)
    {
        this.onResize = onResize;
        this.rt = rt;
        prevPos = rt.anchoredPosition;
        prevSize = rt.sizeDelta;
    }
    public void CheckAndSolve()
    {
        if (rt.anchoredPosition != prevPos || rt.sizeDelta != prevSize)
        {
            if (onResize != null) onResize();
        }
        prevPos = rt.anchoredPosition;
        prevSize = rt.sizeDelta;
    }
}