/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：MethodItemBase
 * 简    述： 
 * 创建时间：2015/7/13 21:03:18
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
using System.Collections.Generic;

public class MethodItem
{
    private CodeMemberMethod method;
    public CodeMemberMethod Method { get { return method; } }

    protected internal CodeConditionStatement statement;

    public MethodItem(string inName, MemberAttributes inAtt, List<string> inParameters)
    {
        method = new CodeMemberMethod();
        method.Name = inName;
        method.Attributes = inAtt;
        for (int i = 0; i < inParameters.Count; i++)
        {

            method.Parameters.Add(new CodeParameterDeclarationExpression(inParameters[i], "inArg" + i));
        }
        statement = new CodeConditionStatement();
    }

    public void SetReturn(string s)
    {
        method.ReturnType = new CodeTypeReference(s);
    }
}
