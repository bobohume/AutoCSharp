/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：ItemConstruct
 * 简    述： 
 * 创建时间：2015/9/15 10:26:38
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
using System.Collections.Generic;

/// <summary>
/// 构造函数
/// </summary>
public class ItemConstruct
{
    private CodeConstructor construct;
    /// <summary>
    /// 构造函数
    /// </summary>
    public CodeConstructor Struct { get { return construct; } }

    /// <summary>
    /// 无参
    /// </summary>
    public ItemConstruct()
    {
        construct = new CodeConstructor();
        construct.Attributes = MemberAttributes.Public;
    }

    /// <summary>
    /// 有参
    /// </summary>
    /// <param name="inPars"></param>
    public ItemConstruct(List<string> inPars)
    {
        construct = new CodeConstructor();
        for (int i = 0; i < inPars.Count; i++)
        {
            construct.Parameters.Add(new CodeParameterDeclarationExpression(inPars[i], "inArg" + i));
        }
        construct.Attributes = MemberAttributes.Public;
    }
}