﻿using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;
namespace Esa
{
    [Serializable]
    public class Clip
    {
        [XmlIgnore] public string clipName;
        [XmlIgnore] public Vector2Int frameRange;

        public List<CurveObj> curves;
        public Clip() { curves = new List<CurveObj>(); }
        public Clip(string clipName) { this.clipName = clipName; curves = new List<CurveObj>(); }
        public CurveObj this[TransDOF t]
        {
            get { return GetCurve(t); }
        }
        public CurveObj GetCurve(TransDOF ast)
        {
            foreach (var curve in curves)
            {
                if (curve.ast == ast)
                {
                    return curve;
                }
            }
            return null;
        }
        public CurveObj GetCurve(Bone bone)
        {
            foreach (var curve in curves)
            {
                if (curve.ast.dof.bone == bone)
                {
                    return curve;
                }
            }
            return null;
        }
        public CurveObj this[string name]
        {
            get { return GetCurve(name); }
        }
        public CurveObj GetCurve(string name)
        {
            foreach (var curve in curves)
            {
                if (curve.name == name)
                {
                    return curve;
                }
            }
            throw null;
        }
        public int IndexOf(TransDOF ast)
        {
            for (int i = 0; i < curves.Count; i++)
            {
                if (curves[i].ast == ast) return i;
            }
            return -1;
        }
        public void AddCurve(TransDOF ast)
        {
            if (IndexOf(ast) != -1) throw null;
            curves.Add(new CurveObj(ast));
        }
        public void AddEulerPosAllCurve(int frameIdx)
        {
            var c = 0;
            foreach (var curve in curves)
            {
                var ast = curve.ast;
                if (ast == null) continue;
                curve.AddEulerPos(frameIdx, ast.euler, ast.pos);
                c++;
            }
            Debug.Log("插入到 " + c.ToString() + " 条曲线");
        }
        public void RemoveEulerPosAllCurve(float frameIdx)
        {
            var c = 0;
            foreach (var oc in curves)
            {
                c += oc.RemoveAtTime(frameIdx);
            }
            Debug.Log("删除 " + c.ToString() + " 条曲线");
        }
    }
}