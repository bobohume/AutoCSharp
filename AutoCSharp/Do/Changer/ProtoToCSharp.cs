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
using System.Reflection;

namespace AutoCSharp.Creator
{
    public class ProtoToCSharp : ToCSharpBase
    {
        private ItemMethod toBytes;
        private ItemMethod toStream;

        /// <summary>
        /// .proto -> .cs
        /// </summary>
        /// <param name="inSpace">Pattern.Data.Net</param>
        /// <param name="inClassName"></param>
        /// <param name="inFolderName"></param>
        public ProtoToCSharp(string inSpace, string inClassName, string inFolderName)
            : base(inSpace, inClassName, inFolderName) 
        {
            usingList.Add("System");
            ineritList.Add("Stream");
        }

        public void Create(Dictionary<string, string> inValues)
        {
            foreach (KeyValuePair<string, string> item in inValues)
            {
                fieldList.Add(new ItemField(item.Value, item.Key, "", MemberAttributes.Public));
            }

            toBytes = new ItemMethod("ToBytes", MemberAttributes.Public | MemberAttributes.Override, new List<string>() { });
            toBytes.SetReturn("System.byte[]");
            toBytes.Method.Statements.Add(Line("ByteArray bytes", "new ByteArray()"));

            toStream = new ItemMethod("ToStream", MemberAttributes.Public | MemberAttributes.Static, new List<string>() { "System.byte[]" });
            toStream.SetReturn("Stream");
            toStream.Method.Statements.Add(Line(className + " stream", "new " + className + "()"));
            toStream.Method.Statements.Add(Line("ByteArray data", "new ByteArray(inArg0)"));

            GetItems(inValues).ForEach(i => HandleItem(i));

            toBytes.Method.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("bytes.ToBytes()")));
            toStream.Method.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("stream")));

            methodList.Add(toBytes);
            methodList.Add(toStream);

            Create();
        }

        private List<ProtoToCSItem> GetItems(Dictionary<string, string> inDic)
        {
            string lastValueName = ""; // 上一属性的属性名
            List<ProtoToCSItem> backList = new List<ProtoToCSItem>();
            foreach (KeyValuePair<string, string> item in inDic)
            {
                bool isSelfDefine = false;                                  // 是否为自定义类型
                bool isArray = item.Value.Contains("[]");                   // 是否为数组
                Type t = Assist.GetFieldType(item.Value, ref isSelfDefine); // 属性类型 
                ProtoToCSItem i = new ProtoToCSItem(item.Key, t);
                i.IsSelfDefine = isSelfDefine;
                i.IsArray = isArray;
                i.ArrayLengthName = isArray ? lastValueName : "";
                backList.Add(i);
                lastValueName = item.Key;
            }
            return backList;
        }

        private void HandleItem(ProtoToCSItem inItem)
        {
            if (!inItem.IsArray)
            {
                if (!inItem.IsSelfDefine)   // 非数组非自定义类型 eg. int, uint, byte etc.
                {
                    toStream.Method.Statements.Add(Line("stream." + inItem.Name, "data." + Assist.GetTobytesMethodName(inItem.MType.ToString())));                                                      // to stream
                    if (inItem.MType == typeof(System.String))
                    {
                        CodeExpression ieb = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("bytes"), "Write", new CodeVariableReferenceExpression("this." + inItem.Name + ".Length"));
                        toBytes.Method.Statements.Add(new CodeExpressionStatement(ieb));
                    }
                    CodeExpression ce = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("bytes"), "Write", new CodeVariableReferenceExpression("this." + inItem.Name));                  // to bytes
                    toBytes.Method.Statements.Add(new CodeExpressionStatement(ce));
                }
                else // 非数组自定义类型 eg. vector3unit
                {
                    FieldInfo[] fi = inItem.MType.GetFields();
                    for (int i = 0; i < fi.Length; i++)
                    {
                        toStream.Method.Statements.Add(Line("stream." + inItem.Name + "." + fi[i].Name, "data." + Assist.GetTobytesMethodName(fi[i].FieldType.ToString())));                            // to stream

                        CodeExpression ies = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("bytes"), "Write", new CodeVariableReferenceExpression("this." + inItem.Name + "." + fi[i].Name));
                        toBytes.Method.Statements.Add(ies);                                                                                                                                             // to bytes
                    }
                }
            }
            else // 数组
            {
                toStream.Method.Statements.Add(Line("stream." + inItem.Name, "new " + inItem.MType.ToString().Replace("[]", "[" + inItem.ArrayLengthName + "]")));

                CodeIterationStatement curBytesFor = AddFor(typeof(int), "i", 0, inItem.Name + "." + inItem.ArrayLengthName);
                CodeIterationStatement curStreamFor = AddFor(typeof(int), "i", 0, "stream." + inItem.ArrayLengthName);

                if (inItem.IsSelfDefine) // 自定义数组数据类型 eg. vector3unit[]
                {
                    curStreamFor.Statements.Add(Line("stream." + inItem.Name + "[i]", "new " + inItem.Name + "()"));                                                                                    // to stream
                    FieldInfo[] fi = Type.GetType(inItem.MType.ToString().Replace("[]", "")).GetFields();
                    for (int i = 0; i < fi.Length; i++)
                    {
                        string streamLeft = "stream." + inItem.Name + "[i]." + fi[i].Name;
                        string streamRight = "data." + Assist.GetTobytesMethodName(fi[i].FieldType.ToString());
                        curStreamFor.Statements.Add(Line(streamLeft, streamRight));

                        CodeExpression es = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("bytes"), "Write", new CodeVariableReferenceExpression("this." + inItem.Name + "[i]." + fi[i].Name));
                        curBytesFor.Statements.Add(new CodeExpressionStatement(es));                                                                                                                    // to bytes
                    }
                }
                else // 非自定义数组数据类型 eg. int[]
                {
                    string sleft = "stream." + inItem.Name + "[i]";
                    string sright = "data." + Assist.GetTobytesMethodName(inItem.MType.ToString().Replace("[]",""));
                    curStreamFor.Statements.Add(Line(sleft, sright));                                                                                                                                   // to stream

                    CodeExpression es = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("bytes"), "Write", new CodeVariableReferenceExpression("this." + inItem.Name + "[i]"));
                    curBytesFor.Statements.Add(new CodeExpressionStatement(es));                                                                                                                        // to bytes
                }
                toBytes.Method.Statements.Add(curBytesFor);
                toStream.Method.Statements.Add(curStreamFor);
            }
        }
    }
}
