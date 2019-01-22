using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StrCom = System.StringComparison;
namespace Esa
{
    public static partial class TransTool
    {
        public static bool m_IgnoreCase = true;
        public static bool SearchIgnoreCase
        {
            get { return m_IgnoreCase; }
            set { m_IgnoreCase = value; }
        }
        #region Property go\t\pos\rot\euler
        public static GameObject gameObject(this string target)
        {
            var result = SearchScn(target);
            return result == null ? null : result.gameObject;
        }
        public static Transform transform(this string target)
        {
            return SearchScn(target);
        }
        // these property don't check null,let compiler throw them.
        public static Vector3 position(this string target)
        {
            return SearchScn(target).position;
        }
        public static Quaternion rotation(this string target)
        {
            return SearchScn(target).rotation;
        }
        public static Vector3 eulerAngles(this string target)
        {
            return SearchScn(target).eulerAngles;
        }
        #endregion



        /// <summary>
        /// Transform组件查找：查找所有层级子对象（包括自己），返回第一个匹配
        /// </summary>
        /// 
        public static List<Transform> GetTrans(this Component target, string name, bool includeInactive, bool ignoreCase = true)
        {
            var list = new List<Transform>();
            if (ignoreCase)
            {
                foreach (var c in target.GetComponentsInChildren<Transform>(includeInactive))
                    if (c.name.Equals(name, StrCom.OrdinalIgnoreCase)) list.Add(c);
            }
            else
            {
                foreach (var c in target.GetComponentsInChildren<Transform>(includeInactive))
                    if (c.name == name) list.Add(c);
            }
            return list;
        }
        //search
        public static GameObject Search(this GameObject target, string name, bool ignoreCase)
        {
            Transform result = SearchInternal(target.transform, name, ignoreCase);
            return (result != null) ? result.gameObject : null;
        }
        public static GameObject Search(this GameObject target, string name)
        {
            Transform result = SearchInternal(target.transform, name, m_IgnoreCase);
            return (result != null) ? result.gameObject : null;
        }
        public static Transform Search(this Transform target, string name, bool ignoreCase)
        {
            return SearchInternal(target, name, ignoreCase);
        }
        public static Transform Search(this Component target, string name)
        {
            return SearchInternal(target.transform, name, m_IgnoreCase);
        }
        public static Transform Search(this Transform target, string name)
        {
            return SearchInternal(target, name, m_IgnoreCase);
        }
        public static Transform SearchInternal(this Transform target, string name, bool ignoreCase)
        {
            if (ignoreCase)
            {
                foreach (var c in target.GetComponentsInChildren<Transform>(true))
                    if (c.name.Equals(name, StrCom.OrdinalIgnoreCase)) return c;
            }
            else
            {
                foreach (var c in target.GetComponentsInChildren<Transform>(true))
                    if (c.name == name) return c;
            }
            return null;
        }
        public static Transform SearchRecursive(this Transform target, string name)
        {
            foreach (Transform child in target)
            {
                if (child.name == name)
                    return child;

                Transform result = Search(child, name);
                if (result != null)
                    return result;
            }
            return null;
        }
        public static List<Transform> SearchEndWith(this Transform target, string postfix, bool includeInactive = true)
        {
            List<Transform> match = new List<Transform>();
            foreach (Transform c in target.GetComponentsInChildren<Transform>(includeInactive))
                if (c.name.EndsWith(postfix)) match.Add(c);
            return match;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Transform[] GetComponentsRecursively(this Transform target)
        {
            var list = new List<Transform>();
            foreach (Transform child in target)
            {
                list.Add(child);
                list.AddRange(GetComponentsRecursively(child));
            }
            return list.ToArray();
        }

        public static GameObject Root(string rootname)
        {
            foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
                if (rootname.Equals(go.name, StrCom.OrdinalIgnoreCase))
                {
                    return go;
                }
            return null;
        }

        // 查找包含禁用状态的的对象
        public static GameObject Search(string rootname, string name)
        {
            var result = SearchInternal(rootname, name, m_IgnoreCase);
            return (result == null) ? null : result;
        }
        public static GameObject Search(string rootname, string name, bool ignoreCase)
        {
            var result = SearchInternal(rootname, name, ignoreCase);
            return (result == null) ? null : result;
        }
        public static GameObject SearchInternal(string rootname, string name, bool ignoreCase)
        {
            if (ignoreCase)
            {
                foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
                    if (rootname.Equals(go.name, StrCom.OrdinalIgnoreCase))
                    {
                        GameObject g = go.Search(name, ignoreCase);
                        if (g != null) return g.gameObject;
                    }
            }
            else
            {
                foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
                    if (rootname == go.name)
                    {
                        GameObject g = go.Search(name);
                        if (g != null) return g.gameObject;
                    }
            }
            return null;
        }

        // 查找所有对象
        public static T Search<T>(this GameObject target, string name, bool ignoreCase) where T : Component
        {
            T result = SearchInternal<T>(target.transform, name, ignoreCase);
            return (result != null) ? result : null;
        }
        public static T SearchInternal<T>(this Transform target, string name, bool ignoreCase) where T : Component
        {
            if (ignoreCase)
            {
                foreach (var c in target.GetComponentsInChildren<T>(true))
                    if (c.name.Equals(name, StrCom.OrdinalIgnoreCase)) return c;
            }
            else
            {
                foreach (var c in target.GetComponentsInChildren<T>(true))
                    if (c.name == name) return c;
            }
            return null;
        }
        public static T SearchScene<T>(string name, bool ignoreCase) where T : Component
        {
            GameObject[] GO = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject go in GO)
            {
                T com = go.Search<T>(name, ignoreCase);
                if (com != null) return com;
            }
            return null;
        }
        /// <summary>
        /// 跟 FindObjectOfType<T> 的区别是在于可以包括隐藏的对象，性能未做比较
        /// </summary>
        public static T GetComScene<T>(bool includeInActive = true) where T : Component
        {
            GameObject[] GO = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject go in GO)
            {
                var c = go.GetComponentInChildren<T>(includeInActive);
                if (c != null) return c;
            }
            return null;
        }
        /// <summary>
        /// 跟 FindObjectsOfType<T> 的区别是在于可以包括隐藏的对象，性能未做比较
        /// </summary>
        public static List<T> GetComsScene<T>(bool includeInActive = true) where T : Component
        {
            GameObject[] GO = SceneManager.GetActiveScene().GetRootGameObjects();
            var coms = new List<T>();
            foreach (GameObject go in GO)
            {
                coms.AddRange(go.GetComponentsInChildren<T>(includeInActive));
            }
            return coms;
        }
        // scene root
        public static Transform SearchScnRoot(string name)
        {
            return SearchScnRt(name, m_IgnoreCase);
        }
        public static Transform SearchScnRt(string name, bool ignoreCase)
        {
            var mode = ignoreCase ? StrCom.OrdinalIgnoreCase : StrCom.Ordinal;
            var gos = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject go in gos)
            {
                if (go.name.Equals(name, mode)) return go.transform;
            }
            return null;
        }
        // whole scene
        public static Transform SearchScn(string name, bool ignoreCase)
        {
            GameObject[] GO = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject go in GO)
            {
                GameObject g = go.Search(name, ignoreCase);
                if (g != null) return g.transform;
            }
            return null;
        }
        public static Transform SearchScn(string name)
        {
            return SearchScn(name, m_IgnoreCase);
        }
        // 返回所有匹配名字的子对象
        public static List<Transform> SearchAll(this Transform target, string name)
        {
            List<Transform> match = new List<Transform>();
            foreach (Transform child in target)
            {
                if (child.name == name) match.Add(child);
                match.AddRange(SearchAll(child, name));
            }
            return match;
        }

        public static void SetParent(this GameObject target, Component parent, bool worldStays)
        {
            target.transform.SetParent(parent.transform, worldStays);
        }
        public static void SetParent(this GameObject target, Component parent)
        {
            target.transform.SetParent(parent.transform, true);
        }
        public static void SetParent(this Component target, Component parent, bool worldStays)
        {
            target.transform.SetParent(parent.transform, worldStays);
        }
        public static void SetParent(this Component target, Component parent)
        {
            target.transform.SetParent(parent.transform, true);
        }
        public static void SetParent(this Component target, GameObject parent, bool worldStays)
        {
            target.transform.SetParent(parent.transform, worldStays);
        }
        public static void SetParent(this Component target, GameObject parent)
        {
            target.transform.SetParent(parent.transform, true);
        }


        public static void SetParent(this GameObject target, GameObject parent, bool worldStays)
        {
            target.transform.SetParent(parent.transform, worldStays);
        }
        public static void SetParent(this GameObject target, GameObject parent)
        {
            target.transform.SetParent(parent.transform, true);
        }
        public static void SetParent(this GameObject target, Transform parent, bool worldStays)
        {
            target.transform.SetParent(parent, worldStays);
        }
        public static void SetParent(this GameObject target, Transform parent)
        {
            target.transform.SetParent(parent, true);
        }
        public static void SetParent(this Transform target, GameObject parent, bool worldStays)
        {
            target.SetParent(parent.transform, worldStays);
        }
        public static void SetParent(this Transform target, GameObject parent)
        {
            target.SetParent(parent.transform, true);
        }
        public static void Reset(this GameObject target)
        {
            target.transform.localPosition = Vector3.zero;
            target.transform.localRotation = Quaternion.identity;
            target.transform.localScale = Vector3.one;
        }
        public static void Reset(this Component target)
        {
            target.transform.localPosition = Vector3.zero;
            target.transform.localRotation = Quaternion.identity;
            target.transform.localScale = Vector3.one;
        }
        public static void Reset(this Transform target)
        {
            target.localPosition = Vector3.zero;
            target.localRotation = Quaternion.identity;
            target.localScale = Vector3.one;
        }
        public static float PosX(this string target)
        {
            return target.transform().position.x;
        }
        public static float PosY(this string target)
        {
            return target.transform().position.y;
        }
        public static float PosZ(this string target)
        {
            return target.transform().position.z;
        }
        public static void SetPosX(this string target, float f)
        {
            Transform trans = target.transform();
            trans.position = new Vector3(f, trans.position.y, trans.position.z);
        }
        public static void SetPosY(this string target, float f)
        {
            Transform trans = target.transform();
            trans.position = new Vector3(trans.position.x, f, trans.position.z);
        }
        public static void SetPosZ(this string target, float f)
        {
            Transform trans = target.transform();
            trans.position = new Vector3(trans.position.x, trans.position.y, f);
        }
        public static void PlusPosX(this Transform trans, float f)
        {
            trans.position = new Vector3(trans.position.x + f, trans.position.y, trans.position.z);
        }
        public static void PlusPosY(this Transform trans, float f)
        {
            trans.position = new Vector3(trans.position.x, trans.position.y + f, trans.position.z);
        }
        public static void PlusPosZ(this Transform trans, float f)
        {
            trans.position = new Vector3(trans.position.x, trans.position.y, trans.position.z + f);
        }
        public static void SetPosXY(this Transform trans, Vector2 f)
        {
            trans.position = new Vector3(f.x, f.y, trans.position.z);
        }
        public static void SetPosX(this Transform trans, float f, bool world = true)
        {
            if (world)
                trans.position = new Vector3(f, trans.position.y, trans.position.z);
            else
                trans.localPosition = new Vector3(f, trans.localPosition.y, trans.localPosition.z);
        }
        public static void SetPosY(this Transform trans, float f, bool world = true)
        {
            if (world)
                trans.position = new Vector3(trans.position.x, f, trans.position.z);
            else
                trans.localPosition = new Vector3(trans.localPosition.x, f, trans.localPosition.z);
        }
        public static void SetPosZ(this Transform trans, float f, bool world = true)
        {
            if (world)
                trans.position = new Vector3(trans.position.x, trans.position.y, f);
            else
                trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, f);
        }
        public static void SetPosX(this Transform trans, float f)
        {
            trans.position = new Vector3(f, trans.position.y, trans.position.z);
        }
        public static void SetPosY(this Transform trans, float f)
        {
            trans.position = new Vector3(trans.position.x, f, trans.position.z);
        }
        public static void SetPosZ(this Transform trans, float f)
        {
            trans.position = new Vector3(trans.position.x, trans.position.y, f);
        }
        public static void CopyPosRot(this Transform a, Transform b)
        {
            a.position = b.position;
            a.rotation = b.rotation;
        }
        public static void CopyPosX(this Transform a, Transform b)
        {
            a.SetPosX(b.position.x);
        }
        public static void CopyPosY(this Transform a, Transform b)
        {
            a.SetPosY(b.position.y);
        }
        public static void CopyPosZ(this Transform a, Transform b)
        {
            a.SetPosZ(b.position.z);
        }
        public static void CopyLocalPosX(this Transform a, Transform b)
        {
            a.SetLocalPosX(b.localPosition.x);
        }
        public static void CopyLocalPosY(this Transform a, Transform b)
        {
            a.SetLocalPosY(b.localPosition.y);
        }
        public static void CopyLocalPosZ(this Transform a, Transform b)
        {
            a.SetLocalPosZ(b.localPosition.z);
        }
        public static void CopyUIPosX(this Transform a, Transform b, float factor = 1f)
        {
            a.SetUIPosX((b as RectTransform).anchoredPosition.x * factor);
        }
        public static void CopyUIPosY(this Transform a, Transform b, float factor = 1f)
        {
            a.SetUIPosY((b as RectTransform).anchoredPosition.y * factor);
        }

        public static void SetLocalScaleX(this Component com, float f)
        {
            com.transform.localScale = new Vector3(f, com.transform.localScale.y, com.transform.localScale.z);
        }
        public static void SetLocalScaleY(this Component com, float f)
        {
            com.transform.localScale = new Vector3(com.transform.localScale.x, f, com.transform.localScale.z);
        }
        public static void SetLocalScaleZ(this Component com, float f)
        {
            com.transform.localScale = new Vector3(com.transform.localScale.x, com.transform.localScale.y, f);
        }

        // setpos 
        public static void SetPosXZ(this Component com, Vector3 v)
        {
            com.transform.position = new Vector3(v.x, com.transform.position.y, v.z);
        }
        public static void SetPosXZ(this Component com, float x, float z)
        {
            com.transform.position = new Vector3(x, com.transform.position.y, z);
        }

        // setlocalpos
        public static void SetLocalPosXZ(this Component com, float x, float z)
        {
            com.transform.localPosition = new Vector3(x, com.transform.localPosition.y, z);
        }
        public static void SetLocalPosX(this Component com, float f)
        {
            com.transform.localPosition = new Vector3(f, com.transform.localPosition.y, com.transform.localPosition.z);
        }
        public static void SetLocalPosY(this Component com, float f)
        {
            com.transform.localPosition = new Vector3(com.transform.localPosition.x, f, com.transform.localPosition.z);
        }
        public static void SetLocalPosZ(this Component com, float f)
        {
            com.transform.localPosition = new Vector3(com.transform.localPosition.x, com.transform.localPosition.y, f);
        }
        public static void SetLocalPosX(this Transform trans, float f)
        {
            trans.localPosition = new Vector3(f, trans.localPosition.y, trans.localPosition.z);
        }
        public static void SetLocalPosY(this Transform trans, float f)
        {
            trans.localPosition = new Vector3(trans.localPosition.x, f, trans.localPosition.z);
        }
        public static void SetLocalPosZ(this Transform trans, float f)
        {
            trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, f);
        }
        public static void SetLocalPosX(this GameObject go, float f)
        {
            go.transform.localPosition = new Vector3(f, go.transform.localPosition.y, go.transform.localPosition.z);
        }
        public static void SetLocalPosY(this GameObject go, float f)
        {
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, f, go.transform.localPosition.z);
        }
        public static void SetLocalPosZ(this GameObject trans, float f)
        {
            trans.transform.localPosition = new Vector3(trans.transform.localPosition.x, trans.transform.localPosition.y, f);
        }
        // UI Pos
        public static Vector2 UIPos(this Transform trans)
        {
            return (trans as RectTransform).anchoredPosition;
        }
        public static Vector2 UIPos(this Component com)
        {
            return (com.transform as RectTransform).anchoredPosition;
        }
        public static float UIPosX(this Transform trans)
        {
            return (trans as RectTransform).anchoredPosition.x;
        }
        public static float UIPosY(this Component com)
        {
            return (com.transform as RectTransform).anchoredPosition.y;
        }
        public static void SetUIPos(this Transform trans, Vector2 v)
        {
            (trans as RectTransform).anchoredPosition = v;
        }
        public static void SetUIPos(this Component com, Vector2 v)
        {
            (com.transform as RectTransform).anchoredPosition = v;
        }
        public static void AddUIPosX(this Transform trans, float f)
        {
            var rt = trans as RectTransform;
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x + f, rt.anchoredPosition.y);
        }
        public static void AddUIPosY(this Transform trans, float f)
        {
            var rt = trans as RectTransform;
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y + f);
        }
        public static void SetUIPosX(this Transform trans, float f)
        {
            var rt = trans as RectTransform;
            rt.anchoredPosition = new Vector2(f, rt.anchoredPosition.y);
        }
        public static void SetUIPosY(this Transform trans, float f)
        {
            var rt = trans as RectTransform;
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, f);
        }
        public static void SetUIPosX(this Component com, float f)
        {
            (com.transform as RectTransform).anchoredPosition = new Vector2(f, com.transform.localPosition.y);
        }
        public static void SetUIPosY(this Component com, float f)
        {
            (com.transform as RectTransform).anchoredPosition = new Vector2(com.transform.localPosition.x, f);
        }
        //component
        public static void SetLocalEulerX(this Component com, float f)
        {
            com.transform.localEulerAngles = new Vector3(f, com.transform.localEulerAngles.y, com.transform.localEulerAngles.z);
        }
        public static void SetLocalEulerY(this Component com, float f)
        {
            com.transform.localEulerAngles = new Vector3(com.transform.localEulerAngles.x, f, com.transform.localEulerAngles.z);
        }
        public static void SetLocalEulerZ(this Component com, float f)
        {
            com.transform.localEulerAngles = new Vector3(com.transform.localEulerAngles.x, com.transform.localEulerAngles.y, f);
        }
        //transform
        public static void SetLocalEulerX(this Transform trans, float f)
        {
            trans.localEulerAngles = new Vector3(f, trans.localEulerAngles.y, trans.localEulerAngles.z);
        }
        public static void SetLocalEulerY(this Transform trans, float f)
        {
            trans.localEulerAngles = new Vector3(trans.localEulerAngles.x, f, trans.localEulerAngles.z);
        }
        public static void SetEulerZ(this Transform trans, float f)
        {
            trans.eulerAngles = new Vector3(trans.eulerAngles.x, trans.eulerAngles.y, f);
        }
        public static void SetLocalEulerZ(this Transform trans, float f)
        {
            trans.localEulerAngles = new Vector3(trans.localEulerAngles.x, trans.localEulerAngles.y, f);
        }
        public static void SetLocalRotX(this Transform trans, float f)
        {
            trans.localRotation = new Quaternion(f, trans.localRotation.y, trans.localRotation.z, trans.localRotation.w);
        }
        public static void SetLocalRotY(this Transform trans, float f)
        {
            trans.localRotation = new Quaternion(trans.localRotation.x, f, trans.localRotation.z, trans.localRotation.w);
        }
        public static void SetLocalRotZ(this Transform trans, float f)
        {
            trans.localRotation = new Quaternion(trans.localRotation.x, trans.localRotation.y, f, trans.localRotation.w);
        }
        public static void SetLocalRotW(this Transform trans, float f)
        {
            trans.localRotation = new Quaternion(trans.localRotation.x, trans.localRotation.y, trans.localRotation.z, f);
        }
        public static void SetScaleX(this Transform trans, float f)
        {
            trans.localScale = new Vector3(f, trans.localScale.y, trans.localScale.z);
        }
        public static void SetScaleY(this Transform trans, float f)
        {
            trans.localScale = new Vector3(trans.localScale.x, f, trans.localScale.z);
        }
        public static void SetScaleZ(this Transform trans, float f)
        {
            trans.localScale = new Vector3(trans.localScale.x, trans.localScale.y, f);
        }
        /// <summary>
        /// Reset
        /// </summary>    
        public static void ResetRot(this GameObject target)
        {
            target.transform.localRotation = Quaternion.identity;
        }
        public static void ResetRot(this Transform target)
        {
            target.localRotation = Quaternion.identity;
        }
        public static void ResetRot(this Component target)
        {
            target.transform.localRotation = Quaternion.identity;
        }
        public static void ResetRotScl(this GameObject target)
        {
            target.transform.localRotation = Quaternion.identity;
            target.transform.localScale = Vector3.one;
        }
        public static void ResetRotScl(this Transform target)
        {
            target.localRotation = Quaternion.identity;
            target.localScale = Vector3.one;
        }
        public static void ResetRotScl(this Component target)
        {
            target.transform.localRotation = Quaternion.identity;
            target.transform.localScale = Vector3.one;
        }
        public static void ResetTransform(this GameObject target)
        {
            target.transform.localPosition = Vector3.zero;
            target.transform.localRotation = Quaternion.identity;
            target.transform.localScale = Vector3.one;
        }
        public static void ResetTransform(this Transform target)
        {
            target.localPosition = Vector3.zero;
            target.localRotation = Quaternion.identity;
            target.localScale = Vector3.one;
        }
        public static void ResetTransform(this Component target)
        {
            target.transform.localPosition = Vector3.zero;
            target.transform.localRotation = Quaternion.identity;
            target.transform.localScale = Vector3.one;
        }
    }
}