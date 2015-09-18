/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：CreatePBData
 * 简    述：
 * 创建时间：2015/8/14 9:14:39
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace AutoCSharp.Creator
{
    public class CreatePBData : ToCSharpBase
    {
        public CreatePBData(string inSpace, string inClassName, string inFolderName)
            : base(inSpace, inClassName, inFolderName) { }

        public void SetValue(List<string> inNames)
        {
            usingList.Add("ProtoBuf");
            usingList.Add("System.Collections.Generic");

            classer.CustomAttributes.Add(new CodeAttributeDeclaration("ProtoContract"));

            for (int i = 0; i < inNames.Count; i++)
            {
                string classname = Stringer.FirstLetterUp(inNames[i]);
                ItemField field = new ItemField("Dictionary<string," + classname + ">", classname + "Dic", "new " + "Dictionary<string," + classname + ">()");
                field.SetAttributes(MemberAttributes.Final | MemberAttributes.Public);
                field.AddAttributes("ProtoMember", i + 1);
                fieldList.Add(field);
            }
            Create();
        }
    }
}
