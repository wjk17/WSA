﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace Esa
{
    public enum MB // mouse button
    {
        Left = 0,
        Right = 1,
        Middle = 2,
    };
    public static partial class Events
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
        public static bool KeyDown(params KeyCode[] codes)
        {
            if (used) return false;
            foreach (var code in codes)
            {
                if (Input.GetKeyDown(code)) return true;
            }
            return false;
        }
        public static bool KeyUp(KeyCode code)
        {
            return !used && Input.GetKeyUp(code);
        }
        public static bool Key(KeyCode code)
        {
            return !used && Input.GetKey(code);
        }
        /// <summary>
        /// Input.GetMouseButton
        /// </summary>
        public static bool MouseDown(int button)
        {
            return !used && Input.GetMouseButtonDown(button);
        }
        public static bool MouseDown(MB button)
        {
            return !used && Input.GetMouseButtonDown((int)button);
        }

        public static bool MouseUp(int button)
        {
            return !used && Input.GetMouseButtonUp(button);
        }
        public static bool MouseUp(MB button)
        {
            return !used && Input.GetMouseButtonUp((int)button);
        }
        public static bool Mouse(int button)
        {
            return !used && Input.GetMouseButton(button);
        }
        public static bool Mouse(MB button)
        {
            return !used && Input.GetMouseButton((int)button);
        }
        /// <summary>
        /// mouseUp
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
        /// <summary>
        /// MouseDown
        /// </summary>
        public static bool MouseDown0
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    return Event.current.type == EventType.MouseDown && Event.current.button == 0;
#endif
                return !used && Input.GetMouseButtonDown(0);
            }
        }
        public static bool MouseDown1
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    return Event.current.type == EventType.MouseDown && Event.current.button == 1;
#endif
                return !used && Input.GetMouseButtonDown(1);
            }
        }
        public static bool MouseDown2
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    return Event.current.type == EventType.MouseDown && Event.current.button == 2;
#endif
                return !used && Input.GetMouseButtonDown(2);
            }
        }
        /// <summary>
        /// MouseHold
        /// </summary>
        public static bool Mouse0
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    return Event.current.type == EventType.MouseDrag && Event.current.button == 0;
#endif
                return !used && Input.GetMouseButton(0);
            }
        }
        public static bool Mouse1
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    return Event.current.type == EventType.MouseDrag && Event.current.button == 1;
#endif
                return !used && Input.GetMouseButton(1);
            }
        }
        public static bool Mouse2
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    return Event.current.type == EventType.MouseDrag && Event.current.button == 2;
#endif
                return !used && Input.GetMouseButton(2);
            }
        }
#if UNITY_EDITOR
        public static bool MouseMove
        {
            get
            {
                return Event.current.type == EventType.MouseMove;
            }
        }
        public static Vector3 MousePos
        {
            get
            {
                return Event.current.mousePosition;
            }
        }
        public static Vector3 MousePosLB
        {
            get
            {
                return Event.current.mousePosition.f_sub_y(Screen.height);
            }
        }
#endif
        /// <summary>
        /// Mouse 1to3
        /// </summary>
        public static bool MouseDown1to3
        {
            get { return MouseDown0 || MouseDown1 || MouseDown2; }
        }
        public static bool MouseUp1to3
        {
            get { return MouseUp0 || MouseUp1 || MouseUp2; }
        }
        public static bool Mouse1to3
        {
            get { return Mouse0 || Mouse1 || Mouse2; }
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
        public static bool mouseDown0;
        public static bool mouseDown1;
        public static bool mouseDown2;
        public static bool mouseUp0;
        public static bool mouseUp1;
        public static bool mouseUp2;
        public static bool mouse0;
        public static bool mouse1;
        public static bool mouse2;
        public static bool mouseDown1to3;
        public static bool mouseUp1to3;
        public static bool mouse1to3;
        internal static void Update()
        {
            mouseDown0 = MouseDown0;
            mouseDown1 = MouseDown1;
            mouseDown2 = MouseDown2;
            mouseUp0 = MouseUp0;
            mouseUp1 = MouseUp1;
            mouseUp2 = MouseUp2;
            mouse0 = Mouse0;
            mouse1 = Mouse1;
            mouse2 = Mouse2;
            mouseDown1to3 = MouseDown1to3;
            mouseUp1to3 = MouseUp1to3;
            mouse1to3 = Mouse1to3;
        }
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
    }
}