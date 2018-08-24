using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class LogIns
{
    public string log = "";
    public void Tab(string[] str)
    {
        foreach (var s in str)
        {
            log = log.Length == 0 ? s : log + "\t" + s;
        }
    }
    public void Tab(string str)
    {
        log = log.Length == 0 ? str : log + "\t" + str;
    }
    public void Add(string[] str)
    {
        foreach (var s in str)
        {
            log = log.Length == 0 ? s : log + "\r\n" + s;
        }
    }
    public void Add(string str)
    {
        log = log.Length == 0 ? str : log + "\r\n" + str;
    }
    public void Print()
    {
        Debug.Log(log.Length != 0 ? log : "没有内容");
    }
    public void Print(string str)
    {
        log = str + log;
        Print();
    }
    public void PrintRN(string str)
    {
        log = str + "\r\n" + log;
        Print();
        log = "";
    }
    public bool Dirty()
    {
        return log.Length > 0;
    }
    public void Clear()
    {
        log = "";
    }
}

public static class Log
{
    public static void Print(this GUIStyle style)
    {
        LogIns log = new LogIns();
        log.Add("name: " + style.name);
        log.Add("alignment: " + style.alignment);
        log.Print();
    }
    public static string emptyString = string.Empty;
    public static string NotNull(this string str)
    {
        return str != null ? str : "";// emptyString;
    }
    public static void Print(this Type t)
    {
        PrintInstanceInfor(t);
    }
    public static void PrintInstanceInfor(Type t)
    {
        foreach (MemberInfo member in t.GetMembers())
        {
            member.Name.Add();
        }
        Print();
        foreach (MethodInfo method in t.GetMethods())
        {
            method.Name.Add();
        }
        Print();
        foreach (PropertyInfo property in t.GetProperties())
        {
            property.Name.Add();
        }
        Print();
    }
    static string log;
    public static void Add(this string str)
    {
        log += "\r\n" + str;
    }
    public static void Print()
    {
        Debug.Log(log);
    }
    public static float Print(this float f)
    {
        Debug.Log(f.ToString() + "\r\n");
        return f;
    }
    public static int Print(this int i)
    {
        Debug.Log(i.ToString() + "\r\n");
        return i;
    }
    public static string Print(this string str)
    {
        if (str == null)
            Debug.Log("null String");
        else if (str.Length == 0)
            Debug.Log("empty String");
        else
            Debug.Log(str + "\r\n");
        return str;
    }
    public static string Print(this string str, string start)
    {
        Debug.Log(start + "\r\n" + str);
        return str;
    }
}
