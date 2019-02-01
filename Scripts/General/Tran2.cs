using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    /// <summary>
    /// 包含Position和Rotation的类型
    /// 用于快捷操作Transform，缩短成员名
    /// </summary>
    [Serializable]
    public class Tran2 : ICloneable
    {
        public new string ToString()
        {
            return pos.ToString() + rot.ToString();
        }
        public Tran2 AddZ(float z)
        {
            var n = copy;
            n.pos.z += z;
            return n;
        }
        public Tran2 SetZ(float z)
        {
            var n = copy;
            n.pos.z = z;
            return n;
        }
        public Tran2 SetPos(Vector3 pos)
        {
            var n = copy;
            n.pos = pos;
            return n;
        }
        public Tran2 copy
        {
            get { return (Tran2)Clone(); }
        }
        public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
        {
            return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
        }
        public static implicit operator Tran2(Transform t)
        {
            return new Tran2(t);
        }
        public static implicit operator Matrix4x4(Tran2 t)
        {
            return Matrix4x4.TRS(t.pos, t.rot, Vector3.one);
        }
        public static Quaternion QuaternionFromMatrix2(Matrix4x4 m)
        {
            // Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
            Quaternion q = new Quaternion();
            q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2;
            q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2;
            q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2;
            q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2;
            q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
            q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
            q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));
            return q;
        }
        public static Tran2 operator *(Tran2 t, float f)
        {
            var pos = t.pos * f;
            var rot = Quaternion.Lerp(Quaternion.identity, t.rot, f);
            return new Tran2(pos, rot);
        }
        public static Tran2 operator *(Matrix4x4 matrix, Tran2 t)
        {
            var pos = matrix.MultiplyPoint3x4(t.pos);
            var rot = QuaternionFromMatrix2(matrix);
            return new Tran2(pos, rot * t.rot);
        }
        public static Tran2 operator /(Matrix4x4 matrix, Tran2 t)
        {
            var m = matrix.inverse;
            var pos = m.MultiplyPoint3x4(t.pos);
            var rot = QuaternionFromMatrix2(m);
            return new Tran2(pos, rot * t.rot);
        }
        public object Clone()
        {
            return new Tran2(pos, rot);
        }
        public const bool Local = true;
        public const bool World = false;
        public Vector3 pos;
        public Quaternion rot;
        public static Tran2 Absolute(Transform parent, Tran2 local)
        {
            Tran2 t2 = new Tran2();
            t2.pos = parent.TransformPoint(local.pos);
            t2.rot = parent.rotation * local.rot;
            return t2;
        }
        // 无视原本父子关系，计算指定的“父子”对象之间的相对坐标和旋转值。
        public static Tran2 Relative(Transform parent, Transform child)
        {
            var localPos = parent.InverseTransformPoint(child.position);
            var localRot = Quaternion.Inverse(parent.rotation) * child.rotation;
            return new Tran2(localPos, localRot);
        }
        //public static Tran2 Relative(Vector3 parentPos, Transform child)
        //{
        //    var localPos = parentPos += child.position;
        //    var localRot = Quaternion.Inverse(parent.rotation) * child.rotation;
        //    return new Tran2(localPos, localRot);
        //}
        public static Tran2 Lerp(Tran2 t2, Transform t, float time)
        {
            var nt = new Tran2();
            nt.pos = Vector3.Lerp(t2.pos, t.position, time);
            nt.rot = Quaternion.Lerp(t2.rot, t.rotation, time);
            return nt;
        }
        public static Tran2 Lerp(Transform t, Tran2 t2, float time)
        {
            var nt = new Tran2();
            nt.pos = Vector3.Lerp(t.position, t2.pos, time);
            nt.rot = Quaternion.Lerp(t.rotation, t2.rot, time);
            return nt;
        }
        public static Tran2 Lerp(Tran2 t1, Tran2 t2, float time)
        {
            var nt = new Tran2();
            nt.pos = Vector3.Lerp(t1.pos, t2.pos, time);
            nt.rot = Quaternion.Lerp(t1.rot, t2.rot, time);
            return nt;
        }
        public static Tran2 Lerp(Transform t1, Transform t2, float time)
        {
            var nt = new Tran2();
            nt.pos = Vector3.Lerp(t1.position, t2.position, time);
            nt.rot = Quaternion.Lerp(t1.rotation, t2.rotation, time);
            return nt;
        }
        public static Tran2 Lerp(Component c2, Component c, float time)
        {
            var nt = new Tran2();
            nt.pos = Vector3.Lerp(c2.transform.position, c.transform.position, time);
            nt.rot = Quaternion.Lerp(c2.transform.rotation, c.transform.rotation, time);
            return nt;
        }
        public Tran2()
        {
            pos = Vector3.zero;
            rot = Quaternion.identity;
        }
        public Tran2(Transform transform)
        {
            pos = transform.position;
            rot = transform.rotation;
        }
        public Tran2(Transform transform, bool local)
        {
            pos = local ? transform.localPosition : transform.position;
            rot = local ? transform.localRotation : transform.rotation;
        }
        public Tran2(Vector3 position, Quaternion rotation)
        {
            pos = position;
            rot = rotation;
        }
        public Tran2 Mirror()
        {
            pos = pos.MirrorX();
            rot = rot.Mirror();
            return this;
        }
    }
}