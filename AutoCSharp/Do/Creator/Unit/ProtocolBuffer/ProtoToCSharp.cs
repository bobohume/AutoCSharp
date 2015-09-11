/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：ProtoToCSharp
 * 简    述： 
 * 创建时间：2015/8/20 10:25:20
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace AutoCSharp.Creator
{
    public class ProtoToCSharp : CSharpBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inSpace">Pattern.Data.Net</param>
        /// <param name="inClassName"></param>
        /// <param name="inFolderName"></param>
        public ProtoToCSharp(string inSpace, string inClassName, string inFolderName)
            : base(inSpace, inClassName, inFolderName) 
        {
            usingList.Add("System");
            usingList.Add("Library.Utils");
            usingList.Add("UnityEngine");
            ineritList.Add("Stream");
        }

        public void Create(Dictionary<string, string> inValues)
        {
            List<string> members = new List<string>();
            foreach (KeyValuePair<string, string> i in inValues)
            {
                members.Add(i.Key);
                fieldList.Add(new FieldItem("System." + i.Value, i.Key, "", MemberAttributes.Public));
            }


            MethodItem toBytes = new MethodItem("ToBytes", MemberAttributes.Public | MemberAttributes.Override, new List<string>() { });
            toBytes.SetReturn("System.byte[]");
            toBytes.Method.Statements.Add(Line("ByteArray bytes", "new ByteArray()"));

            MethodItem toStream = new MethodItem("ToStream", MemberAttributes.Public | MemberAttributes.Static, new List<string>(){"System.byte[]"});
            toStream.SetReturn("Stream");
            toStream.Method.Statements.Add(Line(className + " stream", "new " + className + "()"));
            toStream.Method.Statements.Add(Line("ByteArray data", "new ByteArray(inArg0)"));


            members.ForEach(delegate(string s) 
            {
                CodeExpression invokeExpression = new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression("bytes"),
                    "Write", new CodeVariableReferenceExpression("this." + s + ""));

                toBytes.Method.Statements.Add(new CodeExpressionStatement(invokeExpression));

                toStream.Method.Statements.Add(Line("stream." + s, "data.ReadUnsignedInt32()"));

            });

            toBytes.Method.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("bytes.ToBytes()")));
            toStream.Method.Statements.Add(Line("stream.Result", "data.ReadByte()"));
            toStream.Method.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("stream")));

            methodList.Add(toBytes);
            methodList.Add(toStream);

            Create();
        }
    }
}
