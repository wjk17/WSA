using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class KeyWrap
    {
        public KeyCode[] keys;
        public bool Hold()
        {
            foreach (var key in keys)
            {
                if (Input.GetKey(key))
                    return true;
            }
            return false;
        }
        public KeyWrap(params KeyCode[] keys)
        {
            this.keys = keys;
        }
    }
    public class KeyMgr : Singleton<KeyMgr>
    {
        public bool dirRev = true;
        public static List<KeyWrap> keys;
        void Awake()
        {
            keys = new List<KeyWrap>();
            keys.Add(new KeyWrap(KeyCode.W, KeyCode.UpArrow));
            keys.Add(new KeyWrap(KeyCode.A, KeyCode.LeftArrow));
            keys.Add(new KeyWrap(KeyCode.S, KeyCode.DownArrow));
            keys.Add(new KeyWrap(KeyCode.D, KeyCode.RightArrow));
        }
        public static int Dir
        {
            get
            {
                return I.dirRev ? dir * -1 : dir;
            }
        }
        public static bool Idle
        {
            get
            {
                return dir == -1;
            }
        }
        public static bool Move
        {
            get
            {
                return dir != -1;
            }
        }
        public static int dir
        {
            get
            {
                var w = keys[0].Hold();
                var a = keys[1].Hold();
                var s = keys[2].Hold();
                var d = keys[3].Hold();
                int dir = -1;
                if (w && a) dir = 0;
                else if (a && s) dir = 2;
                else if (s && d) dir = 4;
                else if (d && w) dir = 6;
                else if (a) dir = 1;
                else if (s) dir = 3;
                else if (d) dir = 5;
                else if (w) dir = 7;
                return dir;
            }
        }
    }
}