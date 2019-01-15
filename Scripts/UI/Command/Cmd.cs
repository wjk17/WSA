using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa._UI
{
    [Serializable]
    public class Cmd
    {
        public int order;
        public float secondOrder;
        public int insertOrder;
        public object[] args;
        public override string ToString()
        {
            var str = "paraCount: " + args.Length + "\r\n";
            foreach (var arg in args)
            {
                str += ToStr(arg) + "\r\n";
            }
            return str;
        }
        string ToStr(object arg)
        {
            if (arg.GetType() == typeof(Vector3[]))
            {
                return ((Vector3[])arg).ToStrApprox();
            }
            else if (arg.GetType() == typeof(Vector2[]))
            {
                return ((Vector2[])arg).ToStrApprox();
            }
            else if (arg.GetType() == typeof(Vector2))
            {
                return ((Vector2)arg).ToString();
            }
            else if (arg.GetType() == typeof(float))
            {
                return ((float)arg).ToString();
            }
            if (arg.GetType() == typeof(Color))
            {
                return ((Color)arg).ToString();
            }
            else if (arg.GetType() == typeof(bool))
            {
                return ((bool)arg).ToString();
            }
            return "";
        }
    }
}
