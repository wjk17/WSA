﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StrCom = System.StringComparison;
public static class StringTool
{
    public static int SortList(string a, string b)
    {
        int na, nb;
        bool ea = a.GetEndNum(out na);
        bool eb = b.GetEndNum(out nb);
        if (ea && !eb) return 1;
        else if (!ea && eb) return -1;
        else if (!ea && !eb) return a.CompareTo(b);
        else
        {
            if (na > nb) return 1;
            if (na < nb) return -1;
        }

        //if ((!ea && eb) || na > nb) { return 1; } ///顺序从低到高
        //else if ((ea && !eb) || na < nb) { return -1; }
        return a.CompareTo(b);
    }
    /// <summary>
    /// cut string 
    /// </summary>
    public static string CutLeft(this string str, int len)
    {
        return str.Substring(len);
    }
    public static void CutLeft(ref string str, int len)
    {
        str = str.Substring(len);
    }
    public static string CutRight(this string str, int len)
    {
        return str.Substring(0, str.Length - len);
    }
    public static void CutRight(ref string str, int len)
    {
        str = str.Substring(0, str.Length - len);
    }

    /// <summary>
    /// left(len) right(len)
    /// </summary>
    public static string Right(this string str, int len)
    {
        return str.Substring(str.Length - len);
    }
    public static void Right(ref string str, int len)
    {
        str = str.Substring(str.Length - len);
    }
    public static string Left(this string str, int len)
    {
        return str.Substring(0, len);
    }
    public static void Left(ref string str, int len)
    {
        str = str.Substring(0, len);
    }
    public static bool GetEndNum(this string str, out int n)
    {
        var fac = 1;
        n = 0;
        while (true)
        {
            if (str.Length <= 1) break;
            int result;
            if (!int.TryParse(str.Right(1), out result))
            {
                if (fac == 1) return false;
                else return true;
            }
            n += result * fac;
            fac *= 10;
            str = str.CutRight(1);
        }
        if (fac == 1) return false;
        else return true;
    }
    public static bool EndWithIC(this string str, string target)
    {
        return str.EndsWith(target, StrCom.OrdinalIgnoreCase);
    }
    public static string ReplaceEnd(this string str, string target, string replacement)
    {
        if (str.Length < target.Length) return str;
        if (str.EndsWith(target, StrCom.OrdinalIgnoreCase))
        {
            str = str.Substring(0, str.Length - target.Length) + replacement;
        }
        return str;
    }
    public static bool EndsWithArray(this string str, params string[] strs)
    {
        foreach (string s in strs)
        {
            if (str.EndsWith(s))
                return true;
        }
        return false;
    }
    public static bool StartsWithArray(this string str, params string[] strs)
    {
        foreach (string s in strs)
        {
            if (str.StartsWith(s))
                return true;
        }
        return false;
    }
    /// <summary>
    /// 大驼峰（首字母大写）
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string BigCamel(this string str)
    {
        if (str.Length == 1)
        {
            return str.ToUpper();
        }
        else if (str.Length > 1)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
        else
        {
            return str;
        }
    }
}
