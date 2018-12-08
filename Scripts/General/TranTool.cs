using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa
{
    /// <summary>
    /// 包含Position和Euler Rotation的类型
    /// 用于快捷操作Transform，缩短成员名
    /// </summary>
    [Serializable]
    public class Tran2E
    {
        public Vector3 pos;
        public Vector3 rot;
        public Tran2E(Vector3 pos, Vector3 rot)
        {
            this.pos = pos;
            this.rot = rot;
        }
    }
    /// <summary>
    ///  Trans快捷操作
    /// </summary>
    public static class TranTool
    {
        public static void SetLocalTrans(this GameObject target, Transform trans)
        {
            target.transform.localPosition = trans.localPosition;
            target.transform.localRotation = trans.localRotation;
        }
        public static void SetLocalTrans3(this GameObject target, Transform trans)
        {
            target.transform.localPosition = trans.localPosition;
            target.transform.localRotation = trans.localRotation;
            target.transform.localScale = trans.localScale;
        }
        public static void SetTran2(this GameObject target, GameObject source, bool local)
        {
            if (local) SetLocalTrans(target, source.transform);
            else SetTran2(target, source.transform);
        }
        public static void SetTran2(this GameObject target, Transform trans, bool local)
        {
            if (local) SetLocalTrans(target, trans);
            else SetTran2(target, trans);
        }
        public static void SetTran2(this GameObject target, GameObject source)
        {
            target.transform.position = source.transform.position;
            target.transform.rotation = source.transform.rotation;
        }
        public static void SetTran2(this GameObject target, Transform trans)
        {
            target.transform.position = trans.position;
            target.transform.rotation = trans.rotation;
        }


        // 默认世界坐标
        public static void SetTran3(this Transform target, Transform transform)
        {
            target.position = transform.position;
            target.rotation = transform.rotation;
            target.localScale = transform.localScale;
        }
        public static void SetTran3Local(this Transform to, Transform from)
        {
            to.localPosition = from.localPosition;
            to.localRotation = from.localRotation;
            to.localScale = from.localScale;
        }
        public static void SetTran2(this Transform target, Transform transform)
        {
            target.position = transform.position;
            target.rotation = transform.rotation;
        }
        public static void SetTran2(this Transform target, Tran2 trans, bool local)
        {
            if (local) SetTran2Local(target, trans);
            else SetTran2(target, trans);
        }
        public static void SetTran2(this Transform target, Tran2 trans)
        {
            target.position = trans.pos;
            target.rotation = trans.rot;
        }
        public static void SetTran2(this Component target, Tran2 trans)
        {
            target.transform.position = trans.pos;
            target.transform.rotation = trans.rot;
        }
        public static void SetTran2Local(this Component target, Tran2 trans)
        {
            target.transform.localPosition = trans.pos;
            target.transform.localRotation = trans.rot;
        }
        public static void SetTran2Local(this Transform target, Tran2 trans)
        {
            target.localPosition = trans.pos;
            target.localRotation = trans.rot;
        }
        public static Tran2 LocalTran2(this Component target)
        {
            return new Tran2(target.transform.localPosition, target.transform.localRotation);
        }
        public static Tran2 Tran2(this Component target)
        {
            return new Tran2(target.transform.position, target.transform.rotation);
        }
        public static Tran2 LocalTran2(this Transform target)
        {
            return new Tran2(target.localPosition, target.localRotation);
        }
        public static Tran2 Tran2(this Transform target)
        {
            return new Tran2(target.position, target.rotation);
        }
        public static Tran3 LocalTran3(this Component target)
        {
            return new Tran3(target.transform.localPosition, target.transform.localRotation, target.transform.localScale);
        }
        public static Tran3 Tran3(this Component target)
        {
            return new Tran3(target.transform.position, target.transform.rotation, target.transform.lossyScale);
        }
        public static Tran3 LocalTran3(this Transform target)
        {
            return new Tran3(target.localPosition, target.localRotation, target.transform.localScale);
        }
        public static Tran3 Tran3(this Transform target)
        {
            return new Tran3(target.position, target.rotation, target.lossyScale);
        }
        public static Tran2 LocMirrorX(this Transform target)
        {
            Vector3 pos = new Vector3(-target.localPosition.x, target.localPosition.y, target.localPosition.z);
            Quaternion quat = new Quaternion(target.localRotation.x, -target.localRotation.y, -target.localRotation.z, target.localRotation.w);
            return new Tran2(pos, quat);
        }
        //private static Tran LocMirrorX( this Transform target )
        //{
        //    Vector3 pos = new Vector3( -target.localPosition.x, target.localPosition.y, target.localPosition.z );
        //    Vector3 euler = new Vector3( target.localEulerAngles.x, -target.localEulerAngles.y, -target.localEulerAngles.z );
        //    return new Tran( pos, Quaternion.Euler( euler ) );
        //}
        public static Tran2 MirrorX(this GameObject target, GameObject pivot)
        {
            var P = new GameObject("pivot");
            P.SetTran2(pivot);
            var origin = target.transform.parent;
            target.SetParent(P, true);
            target.ScaleAbs();
            P.transform.localScale = P.transform.localScale.MirrorX();
            target.SetParent(origin, true);
            UnityEngine.Object.DestroyImmediate(P);
            return new Tran2(target.transform.position, target.transform.rotation);
        }
        public static Tran2 MirrorX(this GameObject target, Transform pivot)
        {
            target.SetParent(pivot, true);
            pivot.localScale = pivot.localScale.MirrorX();
            return new Tran2(target.transform.position, target.transform.rotation);
        }
        public static Tran2 MirrorX(this Transform target, Transform pivot)
        {
            target.SetParent(pivot, true);
            pivot.localScale = pivot.localScale.MirrorX();
            return new Tran2(target.position, target.rotation);
        }
        public static Tran2 MirrorX(this Transform target, bool local)
        {
            if (local) return LocMirrorX(target);
            else return MirrorX(target);
        }
        public static Tran2 MirrorX(this Transform target)
        {
            Vector3 pos = new Vector3(-target.position.x, target.position.y, target.position.z);
            Quaternion quat = new Quaternion(target.rotation.x, -target.rotation.y, -target.rotation.z, target.rotation.w);
            return new Tran2(pos, quat);
        }
        //public static Tran MirrorX( this Transform target )
        //{
        //    Vector3 pos = new Vector3( -target.position.x, target.position.y, target.position.z );
        //    Vector3 euler = new Vector3( target.eulerAngles.x, -target.eulerAngles.y, -target.eulerAngles.z );
        //    return new Tran( pos, Quaternion.Euler( euler ) );
        //}
        public static float fix(float f)
        {
            if (f > 1)
            {
                return fix(f - 1);
            }
            else
            {
                return f;
            }
        }
    }
}