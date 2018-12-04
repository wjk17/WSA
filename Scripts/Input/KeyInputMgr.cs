using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using key = UnityEngine.KeyCode;
namespace Esa
{
    public class KeyInputMgr : Singleton<KeyInputMgr>
    {
        private void Start()
        {
            KeyInput.Init();
        }
    }
    public static class KeyInput
    {
        public static key[] keys;
        public static bool left
        {
            get { return Key(1); }
        }
        public static bool right
        {
            get { return Key(3); }
        }
        public static int dir
        {
            get
            {
                if (left && !right) return -1;
                else if (!left && right) return 1;
                else return 0;
            }
        }
        public static bool KeyDown(int index)
        {
            return Events.KeyDown(keys[index]);
        }
        public static bool Key(int index)
        {
            return Events.Key(keys[index]);
        }
        public static void Init()
        {
            // 初始化键位
            // WASD JKL
            //keys = new key[] { key.W, key.A, key.S, key.D,key.J,key.K,key.L };
            // ↑←↓→ ZXC
            keys = new key[] { key.UpArrow, key.LeftArrow, key.DownArrow, key.RightArrow,
                            key.Z, key.X, key.C };
        }
    }
}