using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
public class InspectorCallBack : Attribute
{
    public InspectorCallBack() { }
}
public class ShowButtonRow : Attribute
{
    public ShowButtonRow() { }
    public ShowButtonRow(params string[] names) { this.names = names; }
    public string[] names;
}
/// <summary>
/// 
/// </summary>
public class ShowButton : Attribute
{
    public ShowButton() { }
    public ShowButton(string name) { this.name = name; }
    public string name;
}
/// <summary>
/// TODO Inline功能，一行显示多个拥有标签的字段，而不是像ShowButtonRow的数组形式
/// </summary>
/// 
public class Inline : Attribute
{
    public Inline() { }
    public Inline(int idx) { this.idx = idx; }
    public int idx = 0;
    public FieldInfo field;
}
public class ShowToggle : Attribute
{
    public ShowToggle() { }
    public ShowToggle(string name) { this.name = name; }
    public string name;
}

#if !UNITY_EDITOR
public class EditorShowButtons<T> {}
#else
//using UnityEditor;
[CustomEditor(typeof(UsageExample))]
public class UsageExampleEditor : E_ShowButtons<UsageExample>
{

}
public class UsageExample { }
/// <summary>
/// Editor Show Buttons
/// </summary>
[CanEditMultipleObjects]
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

        var stsRows = new Dictionary<int, List<Inline>>(); ;
        foreach (var field in fields)
        {
            var st = (ShowToggle)Attribute.GetCustomAttribute(field, typeof(ShowToggle), false) as ShowToggle;
            if (st != null)
            {
                var il = (Inline)Attribute.GetCustomAttribute(field, typeof(Inline), false) as Inline;
                if (il != null)
                {
                    il.field = field;
                    if (!stsRows.ContainsKey(il.idx))
                    {
                        stsRows.Add(il.idx, new List<Inline>());
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
                var st = (ShowToggle)Attribute.GetCustomAttribute(field, typeof(ShowToggle), false) as ShowToggle;
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
            var attri = Attribute.GetCustomAttribute(method, typeof(ShowButton), false) as ShowButton;
            if (attri != null)
            {
                var name = string.IsNullOrEmpty(attri.name) ? method.Name.BigCamel() : attri.name;
                if (GUILayout.Button(name))
                {
                    foreach (var obj in os)
                    {
                        method.Invoke(obj, null);
                    }
                }
                continue;
            }
            var attriBR = Attribute.GetCustomAttribute(method, typeof(ShowButtonRow), false) as ShowButtonRow;
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
