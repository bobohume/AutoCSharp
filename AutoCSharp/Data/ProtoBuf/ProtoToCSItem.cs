/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：ProtoToCSItem
 * 简    述： 
 * 创建时间：2015/9/14 14:03:03
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;

/// <summary>
/// .proto -> .cs 属性项
/// </summary>
public class ProtoToCSItem
{
    private string name;
    /// <summary>
    /// 属性名
    /// </summary>
    public string Name
    {
        get { return name; }
    }

    private Type mType;
    /// <summary>
    /// 
    /// </summary>
    public Type MType
    {
        get { return mType; }
    }

    private bool isArray;
    /// <summary>
    /// 数组
    /// </summary>
    public bool IsArray
    {
        set { isArray = value; }
        get { return isArray; }
    }

    private bool isSelfDefine;
    /// <summary>
    /// 自定义类型
    /// </summary>
    public bool IsSelfDefine
    {
        set { isSelfDefine = value; }
        get { return isSelfDefine; }
    }

    private string arrayLengthName;
    /// <summary>
    /// 数组长度对应的属性名
    /// <para>eg. 用“childCount”来描述数组“ArrayChild”的长度时，“ArrayChild”的长度就从属性名为“childCount”的属性中获得</para>
    /// </summary>
    public string ArrayLengthName
    {
        set { arrayLengthName = value; }
        get { return arrayLengthName; }
    }

    public ProtoToCSItem(string inName, Type inType)
    {
        name = inName;
        mType = inType;
    }
}