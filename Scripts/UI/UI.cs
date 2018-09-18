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
public static class ICBTool
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
    public RectTransform RT;
    public Rt rt;
    public bool mouseOver;
    public string name;
    public Action getInput;
    public int order;
}
public static class UITool
{
    public static Rt GetRt(this RectTransform rt)
    {
        // anchor 和 pivot 都要上下翻转，转成左上角坐标
        var pos = rt.anchorMin.SubY_L(1) * UI.scaler.referenceResolution;
        pos += rt.anchoredPosition.ReverseY();
        pos += -rt.pivot.SubY_L(1) * rt.rect.size;
        return new Rt(pos, rt.rect.size);
    }
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
        return inputCallBacks.Add_R(new InputCallBack(name, updateFunc, order));
    }
    void Awake()
    {
        I.inputCallBacks = new List<InputCallBack>();
    }
    public List<InputCallBack> inputCallBacks;
    public List<InputCallBack> listCalled;
    public Vector2 _mousePositionRef;
    public virtual int SortList(InputCallBack a, InputCallBack b)
    {
        if (a.order > b.order) { return 1; } ///顺序从低到高
        else if (a.order < b.order) { return -1; }
        return a.name.CompareTo(b.name);
    }
    private void ShowInInspector()
    {
        _mousePositionRef = mousePosRef;
    }
    public void Update()
    {
        ShowInInspector();

        Events.used = false;
        inputCallBacks.Sort(SortList);
        listCalled = new List<InputCallBack>();
        foreach (var call in inputCallBacks)
        {
            if (call.getInput != null) call.getInput();
            listCalled.Add(call);
            if (call.RT != null)
            {
                call.rt = call.RT.GetRt();
                call.mouseOver = call.rt.Contains(mousePosRef);
                if (call.RT.gameObject.activeInHierarchy && call.mouseOver) return;
            }
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

    public static Vector2 ToRefLT(Vector2 pos) // input screen pos
    {
        pos *= facterToReference;
        pos.y = scaler.referenceResolution.y - pos.y;
        return pos;
    }
    internal static Vector2 mousePosRef // LT
    {
        get
        {
            var ip = Input.mousePosition;
            ip *= facterToReference;
            ip.y = scaler.referenceResolution.y - ip.y;
            return ip;
        }
    }
    internal static Vector2 mousePosRef_LB // LB
    {
        get
        {
            return Input.mousePosition * facterToReference;
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
    public static bool MouseUp0
    {
        get { return !used && Input.GetMouseButtonUp(0); }
    }
    public static bool MouseUp1
    {
        get { return !used && Input.GetMouseButtonUp(1); }
    }
    public static bool MouseUp2
    {
        get { return !used && Input.GetMouseButtonUp(2); }
    }
    public static bool MouseDown0
    {
        get { return !used && Input.GetMouseButtonDown(0); }
    }
    public static bool MouseDown1
    {
        get { return !used && Input.GetMouseButtonDown(1); }
    }
    public static bool MouseDown2
    {
        get { return !used && Input.GetMouseButtonDown(2); }
    }
    public static bool Mouse0
    {
        get { return !used && Input.GetMouseButton(0); }
    }
    public static bool Mouse1
    {
        get { return !used && Input.GetMouseButton(1); }
    }
    public static bool Mouse2
    {
        get { return !used && Input.GetMouseButton(2); }
    }
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
        get { return Mouse0 || Mouse1 || Mouse2; }
    }
    public static bool MouseDown1to3
    {
        get { return MouseDown0 || MouseDown1 || MouseDown2; }
    }
    public static bool Click
    {
        get
        {
            return MouseDown0 || MouseUp0 ||
                MouseDown1 || MouseUp1 ||
                MouseDown2 || MouseUp2;
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

    internal static string ToString(KeyCode keyCode)
    {
        var i = (int)keyCode;
        switch (keyCode)
        {
            case KeyCode.Alpha0:
            case KeyCode.Alpha1:
            case KeyCode.Alpha2:
            case KeyCode.Alpha3:
            case KeyCode.Alpha4:
            case KeyCode.Alpha5:
            case KeyCode.Alpha6:
            case KeyCode.Alpha7:
            case KeyCode.Alpha8:
            case KeyCode.Alpha9:
                return (i - 48).ToString();

            case KeyCode.Keypad0:
            case KeyCode.Keypad1:
            case KeyCode.Keypad2:
            case KeyCode.Keypad3:
            case KeyCode.Keypad4:
            case KeyCode.Keypad5:
            case KeyCode.Keypad6:
            case KeyCode.Keypad7:
            case KeyCode.Keypad8:
            case KeyCode.Keypad9:
                return (i - 256).ToString();

            case KeyCode.None:
            case KeyCode.Backspace:
            case KeyCode.Delete:
            case KeyCode.Tab:
            case KeyCode.Clear:
            case KeyCode.Return:
            case KeyCode.Pause:
            case KeyCode.Escape:
            case KeyCode.Space:

            case KeyCode.KeypadPeriod:
            case KeyCode.KeypadDivide:
            case KeyCode.KeypadMultiply:
            case KeyCode.KeypadMinus:
            case KeyCode.KeypadPlus:
            case KeyCode.KeypadEnter:
            case KeyCode.KeypadEquals:
            case KeyCode.UpArrow:
            case KeyCode.DownArrow:
            case KeyCode.RightArrow:
            case KeyCode.LeftArrow:
            case KeyCode.Insert:
            case KeyCode.Home:
            case KeyCode.End:
            case KeyCode.PageUp:
            case KeyCode.PageDown:
            case KeyCode.F1:
            case KeyCode.F2:
            case KeyCode.F3:
            case KeyCode.F4:
            case KeyCode.F5:
            case KeyCode.F6:
            case KeyCode.F7:
            case KeyCode.F8:
            case KeyCode.F9:
            case KeyCode.F10:
            case KeyCode.F11:
            case KeyCode.F12:
            case KeyCode.F13:
            case KeyCode.F14:
            case KeyCode.F15:
            case KeyCode.Exclaim:
            case KeyCode.DoubleQuote:
            case KeyCode.Hash:
            case KeyCode.Dollar:
            case KeyCode.Ampersand:
            case KeyCode.Quote:
            case KeyCode.LeftParen:
            case KeyCode.RightParen:
            case KeyCode.Asterisk:
            case KeyCode.Plus:
            case KeyCode.Comma:
            case KeyCode.Minus:
            case KeyCode.Period:
            case KeyCode.Slash:
            case KeyCode.Colon:
            case KeyCode.Semicolon:
            case KeyCode.Less:
            case KeyCode.Equals:
            case KeyCode.Greater:
            case KeyCode.Question:
            case KeyCode.At:
            case KeyCode.LeftBracket:
            case KeyCode.Backslash:
            case KeyCode.RightBracket:
            case KeyCode.Caret:
            case KeyCode.Underscore:
            case KeyCode.BackQuote:
            case KeyCode.A:
            case KeyCode.B:
            case KeyCode.C:
            case KeyCode.D:
            case KeyCode.E:
            case KeyCode.F:
            case KeyCode.G:
            case KeyCode.H:
            case KeyCode.I:
            case KeyCode.J:
            case KeyCode.K:
            case KeyCode.L:
            case KeyCode.M:
            case KeyCode.N:
            case KeyCode.O:
            case KeyCode.P:
            case KeyCode.Q:
            case KeyCode.R:
            case KeyCode.S:
            case KeyCode.T:
            case KeyCode.U:
            case KeyCode.V:
            case KeyCode.W:
            case KeyCode.X:
            case KeyCode.Y:
            case KeyCode.Z:
            case KeyCode.Numlock:
            case KeyCode.CapsLock:
            case KeyCode.ScrollLock:
            case KeyCode.RightShift:
            case KeyCode.LeftShift:
            case KeyCode.RightControl:
            case KeyCode.LeftControl:
            case KeyCode.RightAlt:
            case KeyCode.LeftAlt:
            case KeyCode.LeftCommand:
            //case KeyCode.LeftApple:
            case KeyCode.LeftWindows:
            case KeyCode.RightCommand:
            //case KeyCode.RightApple:
            case KeyCode.RightWindows:
            case KeyCode.AltGr:
            case KeyCode.Help:
            case KeyCode.Print:
            case KeyCode.SysReq:
            case KeyCode.Break:
            case KeyCode.Menu:
            case KeyCode.Mouse0:
            case KeyCode.Mouse1:
            case KeyCode.Mouse2:
            case KeyCode.Mouse3:
            case KeyCode.Mouse4:
            case KeyCode.Mouse5:
            case KeyCode.Mouse6:
            case KeyCode.JoystickButton0:
            case KeyCode.JoystickButton1:
            case KeyCode.JoystickButton2:
            case KeyCode.JoystickButton3:
            case KeyCode.JoystickButton4:
            case KeyCode.JoystickButton5:
            case KeyCode.JoystickButton6:
            case KeyCode.JoystickButton7:
            case KeyCode.JoystickButton8:
            case KeyCode.JoystickButton9:
            case KeyCode.JoystickButton10:
            case KeyCode.JoystickButton11:
            case KeyCode.JoystickButton12:
            case KeyCode.JoystickButton13:
            case KeyCode.JoystickButton14:
            case KeyCode.JoystickButton15:
            case KeyCode.JoystickButton16:
            case KeyCode.JoystickButton17:
            case KeyCode.JoystickButton18:
            case KeyCode.JoystickButton19:

            case KeyCode.Joystick1Button0:
            case KeyCode.Joystick1Button1:
            case KeyCode.Joystick1Button2:
            case KeyCode.Joystick1Button3:
            case KeyCode.Joystick1Button4:
            case KeyCode.Joystick1Button5:
            case KeyCode.Joystick1Button6:
            case KeyCode.Joystick1Button7:
            case KeyCode.Joystick1Button8:
            case KeyCode.Joystick1Button9:
            case KeyCode.Joystick1Button10:
            case KeyCode.Joystick1Button11:
            case KeyCode.Joystick1Button12:
            case KeyCode.Joystick1Button13:
            case KeyCode.Joystick1Button14:
            case KeyCode.Joystick1Button15:
            case KeyCode.Joystick1Button16:
            case KeyCode.Joystick1Button17:
            case KeyCode.Joystick1Button18:
            case KeyCode.Joystick1Button19:

            case KeyCode.Joystick2Button0:
            case KeyCode.Joystick2Button1:
            case KeyCode.Joystick2Button2:
            case KeyCode.Joystick2Button3:
            case KeyCode.Joystick2Button4:
            case KeyCode.Joystick2Button5:
            case KeyCode.Joystick2Button6:
            case KeyCode.Joystick2Button7:
            case KeyCode.Joystick2Button8:
            case KeyCode.Joystick2Button9:
            case KeyCode.Joystick2Button10:
            case KeyCode.Joystick2Button11:
            case KeyCode.Joystick2Button12:
            case KeyCode.Joystick2Button13:
            case KeyCode.Joystick2Button14:
            case KeyCode.Joystick2Button15:
            case KeyCode.Joystick2Button16:
            case KeyCode.Joystick2Button17:
            case KeyCode.Joystick2Button18:
            case KeyCode.Joystick2Button19:

            case KeyCode.Joystick3Button0:
            case KeyCode.Joystick3Button1:
            case KeyCode.Joystick3Button2:
            case KeyCode.Joystick3Button3:
            case KeyCode.Joystick3Button4:
            case KeyCode.Joystick3Button5:
            case KeyCode.Joystick3Button6:
            case KeyCode.Joystick3Button7:
            case KeyCode.Joystick3Button8:
            case KeyCode.Joystick3Button9:
            case KeyCode.Joystick3Button10:
            case KeyCode.Joystick3Button11:
            case KeyCode.Joystick3Button12:
            case KeyCode.Joystick3Button13:
            case KeyCode.Joystick3Button14:
            case KeyCode.Joystick3Button15:
            case KeyCode.Joystick3Button16:
            case KeyCode.Joystick3Button17:
            case KeyCode.Joystick3Button18:
            case KeyCode.Joystick3Button19:

            case KeyCode.Joystick4Button0:
            case KeyCode.Joystick4Button1:
            case KeyCode.Joystick4Button2:
            case KeyCode.Joystick4Button3:
            case KeyCode.Joystick4Button4:
            case KeyCode.Joystick4Button5:
            case KeyCode.Joystick4Button6:
            case KeyCode.Joystick4Button7:
            case KeyCode.Joystick4Button8:
            case KeyCode.Joystick4Button9:
            case KeyCode.Joystick4Button10:
            case KeyCode.Joystick4Button11:
            case KeyCode.Joystick4Button12:
            case KeyCode.Joystick4Button13:
            case KeyCode.Joystick4Button14:
            case KeyCode.Joystick4Button15:
            case KeyCode.Joystick4Button16:
            case KeyCode.Joystick4Button17:
            case KeyCode.Joystick4Button18:
            case KeyCode.Joystick4Button19:

            case KeyCode.Joystick5Button0:
            case KeyCode.Joystick5Button1:
            case KeyCode.Joystick5Button2:
            case KeyCode.Joystick5Button3:
            case KeyCode.Joystick5Button4:
            case KeyCode.Joystick5Button5:
            case KeyCode.Joystick5Button6:
            case KeyCode.Joystick5Button7:
            case KeyCode.Joystick5Button8:
            case KeyCode.Joystick5Button9:
            case KeyCode.Joystick5Button10:
            case KeyCode.Joystick5Button11:
            case KeyCode.Joystick5Button12:
            case KeyCode.Joystick5Button13:
            case KeyCode.Joystick5Button14:
            case KeyCode.Joystick5Button15:
            case KeyCode.Joystick5Button16:
            case KeyCode.Joystick5Button17:
            case KeyCode.Joystick5Button18:
            case KeyCode.Joystick5Button19:

            case KeyCode.Joystick6Button0:
            case KeyCode.Joystick6Button1:
            case KeyCode.Joystick6Button2:
            case KeyCode.Joystick6Button3:
            case KeyCode.Joystick6Button4:
            case KeyCode.Joystick6Button5:
            case KeyCode.Joystick6Button6:
            case KeyCode.Joystick6Button7:
            case KeyCode.Joystick6Button8:
            case KeyCode.Joystick6Button9:
            case KeyCode.Joystick6Button10:
            case KeyCode.Joystick6Button11:
            case KeyCode.Joystick6Button12:
            case KeyCode.Joystick6Button13:
            case KeyCode.Joystick6Button14:
            case KeyCode.Joystick6Button15:
            case KeyCode.Joystick6Button16:
            case KeyCode.Joystick6Button17:
            case KeyCode.Joystick6Button18:
            case KeyCode.Joystick6Button19:

            case KeyCode.Joystick7Button0:
            case KeyCode.Joystick7Button1:
            case KeyCode.Joystick7Button2:
            case KeyCode.Joystick7Button3:
            case KeyCode.Joystick7Button4:
            case KeyCode.Joystick7Button5:
            case KeyCode.Joystick7Button6:
            case KeyCode.Joystick7Button7:
            case KeyCode.Joystick7Button8:
            case KeyCode.Joystick7Button9:
            case KeyCode.Joystick7Button10:
            case KeyCode.Joystick7Button11:
            case KeyCode.Joystick7Button12:
            case KeyCode.Joystick7Button13:
            case KeyCode.Joystick7Button14:
            case KeyCode.Joystick7Button15:
            case KeyCode.Joystick7Button16:
            case KeyCode.Joystick7Button17:
            case KeyCode.Joystick7Button18:
            case KeyCode.Joystick7Button19:

            case KeyCode.Joystick8Button0:
            case KeyCode.Joystick8Button1:
            case KeyCode.Joystick8Button2:
            case KeyCode.Joystick8Button3:
            case KeyCode.Joystick8Button4:
            case KeyCode.Joystick8Button5:
            case KeyCode.Joystick8Button6:
            case KeyCode.Joystick8Button7:
            case KeyCode.Joystick8Button8:
            case KeyCode.Joystick8Button9:
            case KeyCode.Joystick8Button10:
            case KeyCode.Joystick8Button11:
            case KeyCode.Joystick8Button12:
            case KeyCode.Joystick8Button13:
            case KeyCode.Joystick8Button14:
            case KeyCode.Joystick8Button15:
            case KeyCode.Joystick8Button16:
            case KeyCode.Joystick8Button17:
            case KeyCode.Joystick8Button18:
            case KeyCode.Joystick8Button19:
                return keyCode.ToString();

            default:
                throw null;
        }
    }
}