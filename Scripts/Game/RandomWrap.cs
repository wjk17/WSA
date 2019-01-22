using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Esa
{
    public class RandomWrap
    {
        public float probability = 0.001f;
        public float probabilityMin = 0.001f;
        public float probabilityMax = 0.005f;
        public void SetMin()
        {
            probability = probabilityMin;
        }
        public bool Trigger()
        {
            var trigger = Random.value < probability;
            if (trigger) SetMin();
            Delta();
            return trigger;
        }
        //public bool Trigger()
        //{
        //    return Random.value < probability;
        //}
        internal void Delta()
        {
            probability = Mathf.Lerp(probability, probabilityMax, Time.deltaTime);
        }
    }
}