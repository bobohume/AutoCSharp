﻿/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：XmlToCSharp
 * 简    述：将(xml)自定义的数据结构转换成 Protocol Buffer  所用的类
 * 创建时间：2015/8/11 14:24:08
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Xml;

namespace AutoCSharp.Creator
{
    /// <summary>
    /// 将(xml)自定义的数据结构转换成 Protocol Buffer  所用的类
    /// </summary>
    public class XmlToCSharp : ToCSharpBase
    {
        public XmlToCSharp(string inSpace, string inClassName, string inFolderName)
            : base(inSpace, inClassName, inFolderName)
        {
            usingList.Add("ProtoBuf");
        }

        public void SetValue(XmlAttributeCollection inValue)
        {
            constructList.Add(new ItemConstruct());

            // 构造函数
            ItemConstruct construct = new ItemConstruct(new List<string>() { "System.String" });
            construct.Struct.Statements.Add(Line("string[] ss", "inArg0.Split(\'^\')"));

            classer.CustomAttributes.Add(new CodeAttributeDeclaration("ProtoContract"));
            for (int i = 0; i < inValue.Count; i++)
            {
                fieldList.Add(new ItemField(inValue[i].Name, inValue[i].Value, MemberAttributes.Private));

                ItemProperty item = new ItemProperty(inValue[i].Name);
                item.SetGetName();
                item.SetSetName();
                item.SetValueType(inValue[i].Value);
                item.SetModifier(MemberAttributes.Public | MemberAttributes.Final);
                item.SetField("ProtoMember", (i + 1).ToString());
                propertyList.Add(item);

                Type t = Stringer.ToType(inValue[i].Value);

                string right = t == typeof(System.String) ? "ss[" + i + "]" :
                               t == typeof(System.UInt32) ? "uint.Parse(ss[" + i + "])" :
                               t == typeof(System.Single) ? "float.Parse(ss[" + i + "])" : "new " + t.ToString() + "(inValues[" + i + "])";
                construct.Struct.Statements.Add(Line("_" + Stringer.FirstLetterLower(inValue[i].Name), right));
            }
            constructList.Add(construct);
            Create();
        }
    }
}
