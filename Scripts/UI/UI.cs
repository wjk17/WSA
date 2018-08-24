using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum MB
{
    Left = 0,
    Right,
    Middle,
};
[Serializable]
public class InputCallBack
{
    public InputCallBack() { }
    /// <summary>
    /// 降序
    /// </summary>
    public InputCallBack(string name, Action gi, int order = 0)
    { this.name = name; getInput = gi; this.order = order; }
    public string name;
    public Action getInput;
    public int order;
}
public class UI : MonoSingleton<UI>
{
    public static float Epsilon = 0.000001f;
    public InputCallBack Find(string name)
    {
        foreach (var cb in inputCallBacks)
        {
            if (name == cb.name) return cb;
        }
        return null;
    }

    public InputCallBack AddInputCB(string name, Action updateFunc, int order)
    {
        inputCallBacks.Add(new InputCallBack(name, updateFunc, order));
        return inputCallBacks.Last();
    }
    public List<InputCallBack> inputCallBacks
    {
        get
        {
            if (_inputCallBacks == null)
                _inputCallBacks = new List<InputCallBack>();
            return _inputCallBacks;
        }
    }
    List<InputCallBack> _inputCallBacks;
    public virtual int SortList(InputCallBack a, InputCallBack b)
    {
        if (a.order > b.order) { return 1; } ///顺序从低到高
        else if (a.order < b.order) { return -1; }
        return a.name.CompareTo(b.name);
    }
    public void Update()
    {
        Events.used = false;
        inputCallBacks.Sort(SortList);
        foreach (var call in inputCallBacks)
        {
            call.getInput();
            if (Events.used) return;
        }
    }
    private static Canvas _canvas;
    public static Canvas canvas
    {
        get
        {
            if (_canvas == null) _canvas = FindObjectOfType<Canvas>();
            return _canvas;
        }
        set { _canvas = value; }
    }
    private static CanvasScaler _scaler;
    public static CanvasScaler scaler
    {
        get
        {
            if (_scaler == null) _scaler = FindObjectOfType<CanvasScaler>();
            return _scaler;
        }
        set { _scaler = value; }
    }
    public static float facterToRealPixel
    {
        get
        {
            return Screen.width / scaler.referenceResolution.x;
        }
    }
    public static float facterToReference
    {
        get
        {
            return scaler.referenceResolution.x / Screen.width;
        }
    }
    internal static bool MouseOver(params RectTransform[] rts)
    {
        var ip = Input.mousePosition;
        ip *= facterToReference;
        ip.y = scaler.referenceResolution.y - ip.y;
        foreach (var rt in rts)
        {
            var rect = GetAbsRect(rt);
            if (rect.Contains(ip)) return true;
        }
        return false;
    }
    public static Rect GetAbsRect(RectTransform rt)
    {
        var rect = rt.rect;
        rect.position = AbsRefPos2(rt);
        //rect.position = MathTool.ReverseY(rt.anchoredPosition);
        //if (rt.name != "Area")
        //{
        //    //rect.position += Vector2.Scale((rt.parent as RectTransform).anchoredPosition, Vector2.one.SetY(-1));
        //    //rect.position += new Vector2(-rt.pivot.x * rt.rect.width, -(1 - rt.pivot.y) * rt.rect.height);
        //    rect.position = AbsRefPos(rt);
        //}
        return rect;
    }
    public static Vector2 AbsRefPos2(RectTransform rt)
    {
        var rtParent = rt.parent as RectTransform;
        Vector2 posParent = MathTool.ReverseY(rtParent.anchoredPosition);
        Vector2 pos = posParent;
        Vector2 anchorPos;
        var amin = rt.anchorMin;
        amin.y = 1 - amin.y;
        var amax = rt.anchorMax;
        amax.y = 1 - amax.y;
        var omin = rt.offsetMin;
        var omax = rt.offsetMax;

        if (amin == amax) // 九宫格锚点模式
        {
            anchorPos = Vector2.Scale(amin, rtParent.rect.size);
            pos += anchorPos + new Vector2(omin.x, -omax.y);
        }
        else if (amin == new Vector2(0, 0) && amax == new Vector2(1, 0))
        {
            pos.y += rtParent.rect.height;
            pos += MathTool.ReverseY(rt.anchoredPosition);
        }
        else if (amin == new Vector2(0, 0) && amax == new Vector2(0, 1))
        {
            anchorPos = MathTool.ReverseY(rt.anchoredPosition);
            pos += anchorPos;
        }
        else
        {
            pos += new Vector2(omin.x, -omax.y);// rt.anchoredPosition;
        }
        //p.y = scaler.referenceResolution.y - p.y;
        return pos;
    }
}
public static class Events
{
    public static bool used;
    public static void Use()
    {
        used = true;
    }
    public static bool Command
    {
        get
        {
            var command = Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);
            return !used && command;
        }
    }
    public static bool Ctrl
    {
        get
        {
            var ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            return !used && ctrl;
        }
    }
    public static bool Shift
    {
        get
        {
            var shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            return !used && shift;
        }
    }
    public static bool Alt
    {
        get
        {
            var alt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            return !used && alt;
        }
    }
    public static bool KeyDown_Enter
    {
        get
        {
            return !used && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter));
        }
    }
    public static bool KeyDown(KeyCode code)
    {
        return !used && Input.GetKeyDown(code);
    }
    public static bool KeyUp(KeyCode code)
    {
        return !used && Input.GetKeyUp(code);
    }
    public static bool Key(KeyCode code)
    {
        return !used && Input.GetKey(code);
    }
    public static bool MouseDown(int button)
    {
        return !used && Input.GetMouseButtonDown(button);
    }
    public static bool MouseUp(int button)
    {
        return !used && Input.GetMouseButtonUp(button);
    }
    /// <summary>
    /// Input.GetMouseButton
    /// </summary>
    public static bool Mouse(int button)
    {
        return !used && Input.GetMouseButton(button);
    }
    public static bool MouseDown(MB button)
    {
        return !used && Input.GetMouseButtonDown((int)button);
    }
    public static bool MouseUp(MB button)
    {
        return !used && Input.GetMouseButtonUp((int)button);
    }
    public static bool Mouse(MB button)
    {
        return !used && Input.GetMouseButton((int)button);
    }
    public static bool Mouse1to3
    {
        get
        {
            return Mouse(MB.Left) ||
                Mouse(MB.Right) ||
                Mouse(MB.Middle);
        }
    }
    public static bool MouseDown1to3
    {
        get
        {
            return MouseDown(MB.Left) ||
                MouseDown(MB.Right) ||
                MouseDown(MB.Middle);
        }
    }
    public static bool Click
    {
        get
        {
            return MouseDown(MB.Left) || MouseUp(MB.Left) ||
                MouseDown(MB.Right) || MouseUp(MB.Right) ||
                MouseDown(MB.Middle) || MouseUp(MB.Middle);
        }
    }
    internal static float Axis(string v)
    {
        if (used) return 0;
        return Input.GetAxis(v);
    }
    internal static int KeyAplhaDown()
    {
        int i = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1)) return i; i++;
        if (Input.GetKeyDown(KeyCode.Alpha2)) return i; i++;
        if (Input.GetKeyDown(KeyCode.Alpha3)) return i; i++;
        if (Input.GetKeyDown(KeyCode.Alpha4)) return i; i++;
        if (Input.GetKeyDown(KeyCode.Alpha5)) return i; i++;
        if (Input.GetKeyDown(KeyCode.Alpha6)) return i; i++;
        if (Input.GetKeyDown(KeyCode.Alpha7)) return i; i++;
        if (Input.GetKeyDown(KeyCode.Alpha8)) return i; i++;
        if (Input.GetKeyDown(KeyCode.Alpha9)) return i; i++;
        if (Input.GetKeyDown(KeyCode.Alpha0)) return i; return -1;
    }
}