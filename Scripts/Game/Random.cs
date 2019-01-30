using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UR = UnityEngine.Random;
namespace Esa
{
    public class Random
    {
        public static int IntValue(float v)
        {
            return Mathf.RoundToInt(UR.value * v);
        }
        public static float value
        {
            get
            {
                return UR.value;
            }
        }
        public static float Range(float min, float max)
        {
            return UR.Range(min, max);
        }
        public static int Range(Vector2Int range)
        {
            return Range(range.x, range.y);
        }
        public static int Range(int min, int max)
        {
            var range = max - min;
            return min + IntValue(range);
        }
    }
}