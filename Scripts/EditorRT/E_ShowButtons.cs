using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using Esa;
#if UNITY_EDITOR
using UnityEditor;
public class E_ShowButtons<T> : Editor
{
    public BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance;
    public T o;
    public T[] os;
    public MethodInfo GetMethod<T1>(MethodInfo[] methods)
    {
        foreach (var method in methods)
        {
            var sl = (T1)(object)Attribute.GetCustomAttribute(method, typeof(T1), false);
            if (sl != null)
            {
                return method;
            }
        }
        return null;
    }
    public override void OnInspectorGUI()
    {
        o = (T)Convert.ChangeType(target, typeof(T));
        os = new T[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            os[i] = o = (T)Convert.ChangeType(targets[i], typeof(T));
        }

        base.OnInspectorGUI();
        EditorGUILayout.Separator();

        var t = typeof(T);
        var fields = t.GetFields(bindingFlags);
        var methods = t.GetMethods(bindingFlags);

        ////CallBack
        //foreach (var method in methods)
        //{
        //    var sl = GetMethod<InspectorCallBack>(methods);
        //    if (sl != null)
        //    {
        //        sl.Invoke(o, null);
        //    }
        //}

        var stsRows = new Dictionary<int, List<InlineAttribute>>(); ;
        foreach (var field in fields)
        {
            var st = (ToggleAttribute)Attribute.GetCustomAttribute(field, typeof(ToggleAttribute), false) as ToggleAttribute;
            if (st != null)
            {
                var il = (InlineAttribute)Attribute.GetCustomAttribute(field, typeof(InlineAttribute), false) as InlineAttribute;
                if (il != null)
                {
                    il.field = field;
                    if (!stsRows.ContainsKey(il.idx))
                    {
                        stsRows.Add(il.idx, new List<InlineAttribute>());
                    }
                    stsRows[il.idx].Add(il);
                }
                else
                {
                    var prev = (bool)field.GetValue(o);
                    var value = GUILayout.Toggle(prev, string.IsNullOrEmpty(st.name) ? field.Name : st.name, "Button");
                    field.SetValue(o, value);
                    if (prev != value)
                    {
                        foreach (var m in methods)
                        {
                            if (m.Name == field.Name + "_Changed")
                            {
                                m.Invoke(o, new object[] { value });
                                continue;
                            }
                        }
                    }
                }
            }
        }
        foreach (var row in stsRows)
        {
            EditorGUILayout.BeginHorizontal();
            foreach (var stil in row.Value)
            {
                var field = stil.field;
                var st = (ToggleAttribute)Attribute.GetCustomAttribute(field, typeof(ToggleAttribute), false) as ToggleAttribute;
                var prev = (bool)field.GetValue(o);
                var value = GUILayout.Toggle(prev, string.IsNullOrEmpty(st.name) ? field.Name : st.name, "Button");
                field.SetValue(o, value);
                if (prev != value)
                {
                    foreach (var m in methods)
                    {
                        if (m.Name == field.Name + "_Changed")
                        {
                            m.Invoke(o, new object[] { value });
                            continue;
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        foreach (var method in methods)
        {
            var attri = Attribute.GetCustomAttribute(method, typeof(ButtonAttribute), false) as ButtonAttribute;
            if (attri != null)
            {
                var name = string.IsNullOrEmpty(attri.Name) ? method.Name.BigCamel() : attri.Name;
                if (GUILayout.Button(name))
                {
                    foreach (var obj in os)
                    {
                        method.Invoke(obj, null);
                    }
                }
                continue;
            }
            var attriBR = Attribute.GetCustomAttribute(method, typeof(ButtonRowAttribute), false) as ButtonRowAttribute;
            if (attriBR != null)
            {
                EditorGUILayout.BeginHorizontal();
                int i = 0;
                foreach (var name in attriBR.names)
                {
                    if (GUILayout.Button(name))
                    {
                        foreach (var obj in os)
                        {
                            method.Invoke(obj, new object[] { i });
                        }
                    }
                    i++;
                }
                EditorGUILayout.EndHorizontal();
                continue;
            }
        }
    }
}
#endif