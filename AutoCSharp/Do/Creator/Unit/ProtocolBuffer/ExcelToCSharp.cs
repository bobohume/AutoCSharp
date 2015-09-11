/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：ExcelToCSharp
 * 简    述： 
 * 创建时间：2015/8/10 15:16:23
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
using System.Collections.Generic;
namespace AutoCSharp.Creator
{
    public class ExcelToCSharp : CSharpBase
    {
        public ExcelToCSharp(string inSpace, string inClassName, string inFolderName)
            : base(inSpace, inClassName, inFolderName)
        {
            // 引用
            usingList.Add("ProtoBuf");
            usingList.Add("System.Collections.Generic");
            ineritList.Add("IProtoBufable");
        }

        /// <summary>
        /// <para>0: 字段名</para>
        /// <para>1: a+i+0 指明类型及是否为Key值</para>
        /// <para>2: 注释</para>
        /// </summary>
        /// <param name="inList">0: 字段名</param>
        public void SetValue(List<string[]> inList)
        {
            MethodItem mi = new MethodItem("Set", MemberAttributes.Final | MemberAttributes.Public, new List<string>(){"List<string>"});

            classer.CustomAttributes.Add(new CodeAttributeDeclaration("ProtoContract"));

            List<string> keyList = new List<string>();// key 值

            for (int i = 0; i < inList.Count; i++)
            {
                string[] ss = inList[i][1].Split('+');
                string typeString = ss[1];
                CustumType ct = Assist.StrToEnum<CustumType>(typeString, CustumType.None);
                if (ct != CustumType.None) // 基本类型或自定义类型
                {
                    //AddField(inList[i][0], inList[i][1], MemberAttributes.Private);
                    fieldList.Add(new FieldItem(inList[i][0], inList[i][1], MemberAttributes.Private));
                    PropertyItem item = new PropertyItem(inList[i][0]);
                    item.SetGetName();
                    item.SetSetName();
                    item.SetComment(inList[i][2]);
                    item.SetValueType(inList[i][1]);
                    item.SetModifier(MemberAttributes.Public | MemberAttributes.Final);
                    item.SetField("ProtoMember", (i + 1).ToString());
                    propertyList.Add(item);

                    if (ss[2] == "1")// 如果该属性是类的Key值，则加入列表
                    {
                        keyList.Add(inList[i][0]);
                    }

                    Type vType = Assist.stringToType(inList[i][1]);
                    string left = "_" + Assist.FirstLetterLower(inList[i][0]);
                    CodeVariableReferenceExpression right = new CodeVariableReferenceExpression();
                    if (vType == typeof(System.String))
                    {
                        right.VariableName = "inArg0[" + i + "]";
                    }
                    else if (vType == typeof(System.UInt32))
                    {
                        right.VariableName = "uint.Parse(inArg0[" + i + "])";
                    }
                    else if (vType == typeof(System.Single))
                    {
                        right.VariableName = "float.Parse(inArg0[" + i + "])";
                    }
                    else
                    {
                        right.VariableName = "new " + vType.ToString() + "(inArg0[" + i + "])";
                    }
                    CodeAssignStatement ass = new CodeAssignStatement(new CodeVariableReferenceExpression(left), right);
                    mi.Method.Statements.Add(ass);
                }
                else // 从属表
                {
                    string subclassname = Assist.FirstLetterUp(typeString);

                    MethodItem mis = new MethodItem("Get" + subclassname, MemberAttributes.Final | MemberAttributes.Public, new List<string>() { "System.String" , "PBData"});

                    SetComment("获取" + inList[i][2], mis.Method);

                    mis.Method.ReturnType = new CodeTypeReference(subclassname);
                    mis.Method.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("inArg1." + subclassname + "Dic[this.key + \"_\" + inArg0]")));            

                    methodList.Add(mis);
                }
            }

            // Key 属性
            PropertyItem keyPropertyItem = new PropertyItem("key");
            if (keyList.Count == 1)
            {
                keyPropertyItem.SetGetName(keyList[0] + ".ToString()");
            }
            else if (keyList.Count == 2)
            {
                keyPropertyItem.SetGetName(keyList[0] + ".ToString() + \"_\" +" + keyList[1] + ".ToString()");
            }
            else if (keyList.Count == 3)
            {
                keyPropertyItem.SetGetName(keyList[0] + ".ToString() + \"_\" +" + keyList[1] + ".ToString() + \"_\" +" + keyList[2] + ".ToString()");
            }
            keyPropertyItem.SetValueType();
            keyPropertyItem.SetModifier(MemberAttributes.Public | MemberAttributes.Final);
            keyPropertyItem.SetComment("类的Key值");
            propertyList.Add(keyPropertyItem);

            methodList.Add(mi);
            Create();
        }
    }
}
