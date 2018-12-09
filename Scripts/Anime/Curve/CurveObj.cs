using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;
namespace Esa
{
    [Serializable]
    public class CurveXYZ
    {
        public CurveXYZ()
        {
            x = new Curve2();
            y = new Curve2();
            z = new Curve2();
        }
        public Curve2 x;
        public Curve2 y;
        public Curve2 z;
    }
    public enum Curve
    {
        PosX, PosY, PosZ, RotX, RotY, RotZ
    }
    [Serializable]
    public class CurveObj
    {
        public CurveObj() { } // for xmlserialize
        [XmlIgnore] public CurveObj pair;
        public string name;
        //public CurveObj(ASTransDOF ast) => this.ast = ast;
        public CurveObj(TransDOF ast) { name = ast.transform.name; this.ast = ast; pos = new CurveXYZ(); rot = new CurveXYZ(); }
        public TransDOF ast;
        [XmlIgnore] public Curve2[] poss { get { return new Curve2[] { pos.x, pos.y, pos.z }; } }
        [XmlIgnore] public Curve2[] rots { get { return new Curve2[] { rot.x, rot.y, rot.z }; } }
        public CurveXYZ pos;
        public CurveXYZ rot;
        [XmlIgnore] public Curve2[] curves { get { return new Curve2[] { pos.x, pos.y, pos.z, rot.x, rot.y, rot.z }; } }
        public Curve2 Curve(Curve c)
        {
            return curves[(int)c];
        }
        public Vector3 Pos(float time)
        {
            return new Vector3(pos.x.Evaluate1D(time), pos.y.Evaluate1D(time), pos.z.Evaluate1D(time));
        }
        public Vector3 Rot(float time)
        {
            return new Vector3(rot.x.Evaluate1D(time), rot.y.Evaluate1D(time), rot.z.Evaluate1D(time));
        }
        public int RemoveAtTime(float time)
        {
            int c = 0;
            c += RemoveAtTime(rot.x, time);
            c += RemoveAtTime(rot.y, time);
            c += RemoveAtTime(rot.z, time);

            c += RemoveAtTime(pos.x, time);
            c += RemoveAtTime(pos.y, time);
            c += RemoveAtTime(pos.z, time);
            return c;
        }
        int RemoveAtTime(Curve2 curve, float time)
        {
            int c = 0;
            if (curve.keys != null && curve.keys.Count > 0)
            {
                c += RemoveTimeOf(curve, time);
            }
            return c;
        }
        // return remove count
        int RemoveTimeOf(Curve2 curve, float time)
        {
            int c = 0;
            var t = new List<Key2>(curve);
            foreach (var key in t)
            {
                if (MathTool.Approx(key.time, time, 3)) { curve.Remove(key); c++; }
            }
            return c;
        }

        internal Tran2E Tran2E(float time)
        {
            return new Tran2E(Pos(time), Rot(time));
        }

        internal void AddEulerPos(int frameIndex, Vector3 euler, Vector3 pos)
        {
            AddEulerCurve(frameIndex, euler);
            AddPositionCurve(frameIndex, pos);
        }
        public bool Empty()
        {
            foreach (var curve in curves)
            {
                if (curve.Count > 0) return false;
            }
            return true;
        }
        internal void Update(float time)
        {
            ast.pos = Pos(time);
            ast.euler = Rot(time);
            ast.Update();
        }

        public void AddEulerCurve(int frameIndex, Vector3 euler)
        {
            rot.x.InsertKey(new Key2(frameIndex, euler.x));
            rot.y.InsertKey(new Key2(frameIndex, euler.y));
            rot.z.InsertKey(new Key2(frameIndex, euler.z));
        }
        public void AddPositionCurve(int frameIndex, Vector3 pos)
        {
            this.pos.x.InsertKey(new Key2(frameIndex, pos.x));
            this.pos.y.InsertKey(new Key2(frameIndex, pos.y));
            this.pos.z.InsertKey(new Key2(frameIndex, pos.z));
        }
    }
}