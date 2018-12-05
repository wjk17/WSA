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
            Keys.Init();
        }
    }
    public static class Keys
    {
        public static key[] keys;
        public static bool Up { get { return Key(0); } }
        public static bool Left { get { return Key(1); } }
        public static bool Down { get { return Key(2); } }
        public static bool Right { get { return Key(3); } }
        public static bool Melee { get { return Key(4); } }
        public static bool Jump { get { return Key(5); } }
        public static bool UpPress { get { return KeyDown(0); } }
        public static bool LeftPress { get { return KeyDown(1); } }
        public static bool DownPress { get { return KeyDown(2); } }
        public static bool RightPress { get { return KeyDown(3); } }
        public static bool MeleePress { get { return KeyDown(4); } }
        public static bool JumpPress { get { return KeyDown(5); } }
        public static Vector2 DirX
        {
            get
            {
                if (Left && !Right) return Vector2.right * -1;
                else if (!Left && Right) return Vector2.right;
                else return Vector2.zero;
            }
        }
        public static int dirX
        {
            get
            {
                if (Left && !Right) return -1;
                else if (!Left && Right) return 1;
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