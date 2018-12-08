using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    [Serializable]
    public struct Tran3
    {
        public static Tran3 _Origin;
        public static Tran3 Origin
        {
            get
            {
                return new Tran3(Vector3.zero, Quaternion.identity, Vector3.one);
            }
        }
        public Vector3 pos;
        public Quaternion rot;
        public Vector3 scl;
        //public Tran3()
        //{
        //    pos = Vector3.zero;
        //    rot = Quaternion.identity;
        //    scl = Vector3.one;
        //}
        public Tran3(Transform transform)
        {
            pos = transform.position;
            rot = transform.rotation;
            scl = transform.lossyScale;
        }
        public Tran3(Transform transform, bool local)
        {
            pos = local ? transform.localPosition : transform.position;
            rot = local ? transform.localRotation : transform.rotation;
            scl = local ? transform.localScale : transform.lossyScale;
        }
        public Tran3(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            pos = position;
            rot = rotation;
            scl = scale;
        }
        public Tran3 Mirror()
        {
            pos = pos.MirrorX();
            rot = rot.Mirror();
            return this;
        }
        public static implicit operator Tran3(Transform t)
        {
            return new Tran3(t);
        }
        public static implicit operator Matrix4x4(Tran3 t)
        {
            return Matrix4x4.TRS(t.pos, t.rot, t.scl);
        }
    }
}