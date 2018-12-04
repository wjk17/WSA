using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Esa.UI;
namespace Esa
{
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
            return UI.UI.I.inputCallBacks.Add_R(new InputCallBack(mono, null, 0));
        }
        public static InputCallBack AddInputCB(this MonoBehaviour mono, int order)
        {
            return UI.UI.I.inputCallBacks.Add_R(new InputCallBack(mono, null, order));
        }
        public static InputCallBack AddInputCB(this MonoBehaviour mono, Action updateFunc, int order)
        {
            return UI.UI.I.inputCallBacks.Add_R(new InputCallBack(mono, updateFunc, order));
        }
        // 自定义名字，mono为null时rectTransform为null
        public static InputCallBack AddInputCB(this MonoBehaviour mono, string name, Action updateFunc, int order)
        {
            return UI.UI.I.inputCallBacks.Add_R(new InputCallBack(name, updateFunc, order));
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
        public static string def_AxisMouseWheel = "Mouse ScrollWheel";
        public static float AxisMouseWheel
        {
            get { return Axis(def_AxisMouseWheel); }
        }
        public static float Axis(string v)
        {
            if (used) return 0;
            return Input.GetAxis(v);
        }
        public static int KeyAplhaDown()
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
}