/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：Stringer
 * 简    述： 
 * 创建时间：2015/9/15 13:37:01
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.Text.RegularExpressions;

public class Stringer
{
    /// <summary>
    /// 数字返回 True, 其它返回 False
    /// <para>数字前面有一个 +号 或 -号 返回 True</para>
    /// </summary>
    static public bool IsNumber(string inStr)
    {
        return new Regex(@"^[+-]?\d*(,\d{3})*(\.\d+)?$").IsMatch(inStr.Trim());
    }

    /// <summary>
    /// 首字母大写
    /// </summary>
    static public string FirstLetterUp(string s)
    {
        return s.Substring(0, 1).ToUpper() + s.Substring(1);
    }

    /// <summary>
    /// 首字母小写
    /// </summary>
    static public string FirstLetterLower(string inStr)
    {
        return inStr.Substring(0, 1).ToLower() + inStr.Substring(1);
    }

    /// <summary>
    /// 转 Type
    /// </summary>
    static public Type ToType(string inStr)
    {
        Type t = typeof(System.String);
        if (inStr.Contains("+"))// Excel 类型结构 eg.a+i+1
        {
            string[] ss = inStr.Split('+');
            if (ss[1] == "i")
            {
                t = typeof(System.UInt32);
            }
            else if (ss[1] == "f")
            {
                t = typeof(System.Single);
            }
            else if (ss[1] == "t" || ss[1] == "s")
            {

            }
            else
            {
                t = Type.GetType(ss[1]);
            }
        }
        else
        {
            if (inStr != "" && IsNumber(inStr))
            {
                t = inStr.Contains(".") ? typeof(System.Single) : typeof(System.UInt32);
            }
        }
        return t;
    }

    /// <summary>
    /// 字符串转枚举
    /// </summary>
    static public T StrToEnum<T>(string inStr, T inType)
    {
        if (!Enum.IsDefined(typeof(T), inStr))
        {
            return inType;
        }
        return (T)Enum.Parse(typeof(T), inStr);
    }
}