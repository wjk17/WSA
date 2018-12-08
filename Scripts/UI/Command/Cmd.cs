using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa.UI
{
    [Serializable]
    public class Cmd
    {
        public int order;
        public object[] args;
        public override string ToString()
        {
            var str = "Count: " + args.Length + "\r\n";
            foreach (var arg in args)
            {
                str += ToStr(arg) + "\r\n";
            }
            return str;
        }
        string ToStr(object arg)
        {
            if (arg.GetType() == typeof(Vector2))
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
