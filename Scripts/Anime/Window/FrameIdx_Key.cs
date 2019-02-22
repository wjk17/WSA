using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    public class FrameIdx_Key
    {
        public Curve2 curve;
        public FrameIdx_Key(Curve2 curve)
        {
            this.curve = curve;
        }
        private float leftTimer;
        private float rightTimer;
        private float upTimer;
        private float downTimer;
        public float continuousKeyTime = 0.5f; // 上下左右键连发延迟
        public float continuousKeyInterval = 0.01f; // 间隔（其实0.01通常约等于每帧触发）

        public int GetInput(int frameIdx)
        {
            if (Events.Key(KeyCode.LeftArrow))
            {
                leftTimer += Time.deltaTime;
            }
            else { leftTimer = 0; }
            if (Events.Key(KeyCode.RightArrow))
            {
                rightTimer += Time.deltaTime;
            }
            else { rightTimer = 0; }
            if (Events.Key(KeyCode.UpArrow))
            {
                upTimer += Time.deltaTime;
            }
            else { upTimer = 0; }
            if (Events.Key(KeyCode.DownArrow))
            {
                downTimer += Time.deltaTime;
            }
            else { downTimer = 0; }
            if (leftTimer > continuousKeyTime || Events.KeyDown(KeyCode.LeftArrow))
            {
                leftTimer -= continuousKeyInterval;
                frameIdx--;
            }
            else if (rightTimer > continuousKeyTime || Events.KeyDown(KeyCode.RightArrow))
            {
                rightTimer -= continuousKeyInterval;
                frameIdx++;
            }
            else if (upTimer > continuousKeyTime * 1.5f || Events.KeyDown(KeyCode.UpArrow))
            {
                upTimer -= continuousKeyInterval * 1.5f;

                var keys = curve.keys;
                for (int i = keys.Count - 1; i >= 0; i--)
                {
                    var iTime = Mathf.RoundToInt(keys[i].time);
                    if (iTime < frameIdx)
                    {
                        frameIdx = iTime;
                        break;
                    }
                }
            }
            else if (downTimer > continuousKeyTime * 1.5f || Events.KeyDown(KeyCode.DownArrow))
            {
                downTimer -= continuousKeyInterval * 1.5f;
                var keys = curve.keys;
                for (int i = 0; i < keys.Count; i++)
                {
                    var iTime = Mathf.RoundToInt(keys[i].time);
                    if (iTime > frameIdx)
                    {
                        frameIdx = iTime;                        
                        break;
                    }
                }
            }
            return frameIdx;
        }
    }
}