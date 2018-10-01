using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;
public static class ComTool
{
    public static T NameOf<T>(this string name, IList<T> components) where T : Component
    {
        foreach (var com in components)
        {
            if (com.gameObject.name == name)
                return com.GetComponent<T>();
        }
        return default(T);
    }
    public static string GetDirOrCreate(this string dir)
    {
        if (string.IsNullOrEmpty(dir)) return "";
        if (!System.IO.File.Exists(dir))
            System.IO.Directory.CreateDirectory(dir);
        return dir;
    }
    public static void UpdateMeshCol(this GameObject go)
    {
        Mesh m = new Mesh();//"Baked Mesh"
        go.GetComponent<SkinnedMeshRenderer>().BakeMesh(m);
        go.GetComponent<MeshCollider>().sharedMesh = m;
    }
    public static void UpdateMeshCol(this Component com)
    {
        Mesh m = new Mesh();//"Baked Mesh"
        com.GetComponent<SkinnedMeshRenderer>().BakeMesh(m);
        com.GetComponent<MeshCollider>().sharedMesh = m;
    }
    public static GameObject Copy(this Component com, bool keepParent = true)
    {
        var go = com.gameObject;
        var copy = Object.Instantiate(go);
        copy.name = go.name;
        if (keepParent) copy.SetParent(go.transform.parent);
        return copy;
    }
    public static GameObject Copy(this GameObject go, bool keepParent = true)
    {
        var copy = Object.Instantiate(go);
        copy.name = go.name;
        if (keepParent) copy.SetParent(go.transform.parent);
        return copy;
    }
    public static bool ToggleActive(this Component com)
    {
        com.gameObject.SetActive(!com.gameObject.activeSelf);
        return com.gameObject.activeSelf;
    }
    public static bool ToggleActive(this GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
        return gameObject.activeSelf;
    }
    public static void ClearChildren(Transform target, string prefix, bool includeInActive = true)
    {
        var list = new List<Transform>();
        target.GetComponentsInChildren(includeInActive, list);
        list.RemoveAt(0);
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            if (item != null && item.name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                item.parent = null;
                DestroyAuto(item.gameObject);
            }
        }
    }
    public static void ClearChildren(this Transform target, bool includeInActive = true)
    {
        var list = new List<Transform>();
        target.GetComponentsInChildren(includeInActive, list);
        list.RemoveAt(0);
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            if (item != null)
            {
                item.parent = null; // 先移出层级，因为不一定立马删除
                DestroyAuto(item.gameObject);
            }
        }
    }
    public static void Swap<T>(ref T a, ref T b)
    {
        T c = a;
        a = b;
        b = c;
    }
#if UNITY_EDITOR
    public static void DestroyAuto(this Object target)
    {
        if (Application.isPlaying == false)
        {
            Object.DestroyImmediate(target);
        }
        else
        {
            Object.Destroy(target);
        }
    }
#else
    public static void DestroyAuto(this UnityEngine.Object target)
    {
        UnityEngine.Object.Destroy(target);
    }
#endif
    public static T GetComOrAdd<T>(this Component c) where T : Component
    {
        var com = c.GetComponent<T>();
        if (com == null) com = c.gameObject.AddComponent<T>();
        return com;
    }
    public static T GetComOrAdd<T>(this GameObject go) where T : Component
    {
        var com = go.GetComponent<T>();
        if (com == null) com = go.AddComponent<T>();
        return com;
    }
    public static T GetComOrAdd<T>(this Transform t) where T : Component
    {
        var com = t.GetComponent<T>();
        if (com == null) com = t.gameObject.AddComponent<T>();
        return com;
    }
    public static T AddComponent<T>(this Transform t) where T : Component
    {
        var com = t.gameObject.AddComponent<T>();
        return com;
    }
}
