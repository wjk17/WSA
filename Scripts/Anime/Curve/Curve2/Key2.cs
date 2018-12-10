using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;
namespace Esa
{
    /// TODO 插入时 0.4 0.2 0.4 两端持平
    /// 调整time时自动调整保持这个比例
    [Serializable]
    public class Key2
    {
        public Vector2if vector;
        public Vector2 inTan;
        public Vector2 outTan;
        public KeyMode inMode = KeyMode.Bezier;
        public KeyMode outMode = KeyMode.Bezier;
        public Key2()
        {
        }
        public Key2(Vector2 v) : this(v, v, v)
        {
        }
        public Key2(Vector2 v, Vector2 inT, Vector2 outT) : this(v, inT, outT, KeyMode.Bezier, KeyMode.Bezier)
        {
        }
        public Key2(Vector2 v, Vector2 inT, Vector2 outT, KeyMode inMode, KeyMode outMode)
        {
            vector = v;
            inTan = inT;
            outTan = outT;
            this.inMode = inMode;
            this.outMode = outMode;
        }
        public Key2(int frameIdx, float value)
        {
            vector.xi = frameIdx;
            vector.yf = value;
            inTan = vector;
            outTan = vector;
        }
        public Key2(float time, float value)
        {
            vector.xf = time;
            vector.yf = value;
            inTan = vector;
            outTan = vector;
        }
        public Key2 Clone()
        {
            return new Key2(vector, inTan, outTan, inMode, outMode);
        }
        public int idx
        {
            set { vector.xi = value; }
            get { return vector.xi; }
        }
        public float time
        {
            set { vector.xf = value; }
            get { return vector.xf; }
        }
        public float value
        {
            set { vector.yf = value; }
            get { return vector.yf; }
        }
        public Vector2 frameKey
        {
            get { return vector.f.RoundX(); }
            set{ vector = value.RoundX(); }
        }
        public Vector2 inKey
        {
            get { return frameKey + inOS; }
        }
        public Vector2 outKey
        {
            get { return frameKey + outOS; }
        }
        public Vector2 inOS
        {
            get { return inTan - vector.f; }
            set { inTan = vector+= value; }
        }
        public Vector2 outOS
        {
            get { return outTan - vector.f; }
            set { outTan = vector += value; }
        }
        public Vector2 Vector
        {
            get { return vector.f; }
            set { SetVector(value); }
        }
        public void SetVector(Vector2 v)
        {
            var os = v - vector;
            vector = v;
            outTan += os;
            inTan += os;
        }
    }
}