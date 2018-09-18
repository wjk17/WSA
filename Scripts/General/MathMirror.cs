using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathMirror
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
    public static Vector2 SubY_L(this Vector2 v, float L)
    {
        return new Vector2(v.x, L - v.y);
    }
    public static Vector2 SubY_R(this Vector2 v, float R)
    {
        return new Vector2(v.x, v.y - R);
    }
    public static Vector2 AddY_L(this Vector2 v, float L)
    {
        return new Vector2(v.x, L + v.y);
    }
    public static Vector2 AddY(this Vector2 v, float R)
    {
        return new Vector2(v.x, v.y + R);
    }
    public static Vector2 ReverseX(this Vector2 v)
    {
        return v * new Vector2(-1, 1);
    }
    public static Vector2 ReverseY(this Vector2 v)
    {
        return v * new Vector2(1, -1);
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
    public static Vector2 MirrorX(this Vector2 vect, float mirrorAxis)
    {
        vect.x -= mirrorAxis;
        vect.x = -vect.x;
        vect.x += mirrorAxis;
        return vect;
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
