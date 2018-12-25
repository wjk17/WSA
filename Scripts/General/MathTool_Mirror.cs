using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public static partial class MathTool
    {
        // Mirror X
        public static void FlipLocalRotation(this Transform tran)
        {
            tran.localRotation = new Quaternion(tran.localRotation.x, -tran.localRotation.y, -tran.localRotation.z, tran.localRotation.w);
        }
        public static void FlipRotation(this Transform tran)
        {
            tran.rotation = new Quaternion(tran.rotation.x, -tran.rotation.y, -tran.rotation.z, tran.rotation.w);
        }
        public static void FlipLocalPosition(this Transform trans)
        {
            trans.localPosition = new Vector3(-trans.localPosition.x, trans.localPosition.y, trans.localPosition.z);
        }
        public static void FlipPosition(this Transform tran)
        {
            tran.position = new Vector3(-tran.position.x, tran.position.y, tran.position.z);
        }
        // sub
        public static Vector3 f_sub_y(this Vector3 v, float f)
        {
            return new Vector3(v.x, f - v.y, v.z);
        }
        public static Vector2 f_sub_x(this Vector2 v, float f)
        {
            return new Vector2(f - v.x, v.y);
        }
        /// <summary>
        /// (v.x, f - v.y)
        /// </summary>        
        public static Vector2 f_sub_y(this Vector2 v, float f)
        {
            return new Vector2(v.x, f - v.y);
        }
        public static Vector2 y_sub_f(this Vector2 v, float f)
        {
            return new Vector2(v.x, v.y - f);
        }
        /// <summary>
        /// add
        /// </summary>
        public static Vector2 y_add_f(this Vector2 v, float f)
        {
            return new Vector2(v.x, f + v.y);
        }
        public static Vector2 Flip(this Vector2 a)
        {
            return new Vector2(1f - a.x, 1f - a.y);
        }
        public static Vector2 FlipRev(this Vector2 a)
        {
            return -new Vector2(1f - a.x, 1f - a.y);
        }
        public static Vector2 FlipX(this Vector2 a)
        {
            return new Vector2(1f - a.x, a.y);
        }
        public static Vector2 FlipY(this Vector2 a)
        {
            return new Vector2(a.x, 1f - a.y);
        }
        public static Vector2 FlipRevX(this Vector2 a)
        {
            return new Vector2(-(1f - a.x), a.y);
        }
        public static Vector2 FlipRevY(this Vector2 a)
        {
            return new Vector2(a.x, -(1f - a.y));
        }
        public static Vector2 ReverseX(this Vector2 a)
        {
            return new Vector2(-a.x, a.y);
        }
        public static Vector2 ReverseY(this Vector2 a)
        {
            return new Vector2(a.x, -a.y);
        }

        // bounds Mirror
        public static Bounds MirrorX(this Bounds bound)
        {
            bound.center = bound.center.MirrorX();
            var tmp = bound.max;
            bound.max = bound.min;
            bound.min = tmp;
            return bound;
        }
        // v2 mirror
        public static Vector2 MirrorX(this Vector2if vect, float mirrorAxis)
        {
            vect.xf -= mirrorAxis;
            vect.xf = -vect.xf;
            vect.xf += mirrorAxis;
            return vect;
        }
        public static Vector2 MirrorX(this Vector2 vect, float mirrorAxis)
        {
            vect.x -= mirrorAxis;
            vect.x = -vect.x;
            vect.x += mirrorAxis;
            return vect;
        }
        public static bool IsMirrorX(this Vector3 a, Vector3 b)
        {
            return a.x.Approx(-b.x) && a.y.Approx(b.y) && a.z.Approx(b.z);
        }
        public static bool IsMirrorX(this Vector2if a, Vector2if b)
        {
            return a.xf.Approx(-b.xf) && a.yf.Approx(b.yf);
        }
        public static bool IsMirrorX(this Vector2 a, Vector2 b)
        {
            return a.x.Approx(-b.x) && a.y.Approx(b.y);
        }
        public static Vector2 MirrorX(this Vector2 vect)
        {
            return new Vector2(-vect.x, vect.y);
        }
        public static Vector2 MirrorY(this Vector2 vect)
        {
            return new Vector2(vect.x, -vect.y);
        }
        // v3 mirror
        public static Vector3 MirrorX(this Vector3 vect)
        {
            return new Vector3(-vect.x, vect.y, vect.z);
        }
        public static Vector3 MirrorY(this Vector3 vect)
        {
            return new Vector3(vect.x, -vect.y, vect.z);
        }
        public static Vector3 MirrorZ(this Vector3 vect)
        {
            return new Vector3(vect.x, vect.y, -vect.z);
        }
        // mirror Euler
        public static Vector3 MirrorEuler(this Vector3 vect)
        {
            throw null;
            //return Vector3.Scale(new Vector3(vect.x, vect.y, vect.z), ASAvatar.I.globalMirrorFactor);
            //return new Vector3(vect.x, -vect.y, -vect.z);
        }
        public static Vector3 MirrorEulerX(this Vector3 vect)
        {
            return new Vector3(vect.x, -vect.y, -vect.z);
        }
        public static Vector3 MirrorEulerY(this Vector3 vect)
        {
            return new Vector3(-vect.x, vect.y, -vect.z);
        }
        public static Vector3 MirrorEulerZ(this Vector3 vect)
        {
            return new Vector3(-vect.x, -vect.y, vect.z);
        }
        public static Quaternion Mirror(this Quaternion quat)
        {
            return new Quaternion(quat.x, -quat.y, -quat.z, quat.w);
        }
        public static void ScaleAbs(this GameObject target)
        {
            target.transform.localScale = new Vector3(target.transform.localScale.x, target.transform.localScale.y, target.transform.localScale.z);
        }
        public static void ScaleAbs(this Transform target)
        {
            target.localScale = new Vector3(target.localScale.x, target.localScale.y, target.localScale.z);
        }
    }
}