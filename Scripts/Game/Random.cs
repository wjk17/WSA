using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UR = UnityEngine.Random;
namespace Esa
{
    public class Random
    {
        public static float value
        {
            get
            {
                return UR.value;
            }
        }
        // Use this for initialization
        public static float Range(float min, float max)
        {
            return UR.Range(min, max);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}