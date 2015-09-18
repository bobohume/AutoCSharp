/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：CSarpCreater
 * 简    述： 
 * 创建时间：2015/7/14 20:10:02
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace AutoCSharp.Creator
{
    public class Creater
    {
        #region 单例

        private volatile static Creater instance = null;
        private static readonly object locker = new object();
        public static Creater Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new Creater();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion

        /// <summary>
        /// 将指定目录下的xml转成cs文件
        /// </summary>
        /// <param name="inPath">xml所在路径</param>
        /// <param name="inFolderName">根节点名</param>
        /// <param name="inNameSpace">命名空间</param>
        /// <param name="inHeritNames">继承</param>
        /// <returns></returns>
        public bool Xml2CS(string inPath, string inFolderName, string inRootName, string inNameSpace, string inHeritNames)
        {
            Assist.CheckFolderExist(inFolderName);

            Dictionary<string, XmlDocument> doc = Assist.GetXml(inPath);

            foreach (KeyValuePair<string, XmlDocument> item in doc)
            {
                XmlUnit b = new XmlUnit(inNameSpace, item.Key, inFolderName);
                b.SetInherit(inHeritNames);
                b.SetNodeValue(item.Value.SelectSingleNode(inRootName).ChildNodes[0]);
            }
            return true;
        }

        /// <summary>
        /// 将指定目录下的xml转成ProtocolBuffer解析类
        /// </summary>
        /// <param name="inPath"></param>
        /// <param name="inFolderName"></param>
        /// <param name="inRootName"></param>
        /// <returns></returns>
        public bool ProtocolBufferXml(string inPath, string inFolderName, string inRootName)
        {
            Assist.CheckFolderExist(inFolderName);
            Dictionary<string, XmlDocument> doc = Assist.GetXml(inPath);

            foreach (KeyValuePair<string, XmlDocument> item in doc)
            {
                XmlNode rootNode = item.Value.SelectSingleNode(inRootName);

                for (int i = 0; i < rootNode.ChildNodes.Count; i++)
                {
                    XmlNode n = rootNode.ChildNodes[i];
                    XmlToCSharp pbxu = new XmlToCSharp("", n.Name, inFolderName);
                    pbxu.SetValue(n.Attributes);
                }
            }
            return true;
        }

        /// <summary>
        /// Sub Excel -> .cs
        /// </summary>
        /// <param name="inPath"></param>
        /// <param name="inFolderName"></param>
        /// <param name="inNameSpace"></param>
        /// <param name="inHeritNames"></param>
        /// <returns></returns>
        public bool SubExcel(string inPath, string inFolderName, string inNameSpace, string inHeritNames)
        {
            Assist.CheckFolderExist(inFolderName);
            Assist.GetObjPaths(".xls", inPath).ForEach(delegate(string path)
            {
                DataSet ds = Assist.ExcelToData(path);
                DataTable dt = ds.Tables[0];
                ExcelToCSharp e = new ExcelToCSharp(inHeritNames, dt.TableName, inFolderName);
                e.SetInherit(inHeritNames);
                List<string[]> values = new List<string[]>();
                for (int x = 0; x < dt.Columns.Count; x++)
                {
                    string[] v = new string[3];
                    v[0] = dt.Rows[0][x].ToString();
                    v[1] = dt.Rows[2][x].ToString();
                    v[2] = dt.Rows[1][x].ToString();
                    values.Add(v);
                }
                e.SetValue(values);
            });
            return true;
        }

        /// <summary>
        /// Excel -> .cs
        /// </summary>
        /// <param name="inFolderPath">输出文件夹名</param>
        /// <param name="inNameSpace">命名空间</param>
        /// <param name="inHeritNames">继承</param>
        /// <returns></returns>
        public bool Excel(string inPath, string inFolderName, string inNameSpace, string inHeritNames)
        {
            Assist.CheckFolderExist(inFolderName);

            List<string> finalClassNames = new List<string>();
            finalClassNames.Add("SceneLayout");

            Assist.GetObjPaths(".xls", inPath).ForEach(delegate(string path)
            {
                DataSet ds = Assist.ExcelToData(path);

                DataTable dt = ds.Tables[0];

                finalClassNames.Add(dt.TableName);

                ExcelToCSharp e = new ExcelToCSharp(inHeritNames, dt.TableName, inFolderName);
                e.SetInherit(inHeritNames);

                List<string[]> values = new List<string[]>();
                for (int x = 0; x < dt.Columns.Count; x++)
                {
                    string[] v = new string[3];
                    v[0] = dt.Rows[0][x].ToString();
                    v[1] = dt.Rows[2][x].ToString();
                    v[2] = dt.Rows[1][x].ToString();
                    values.Add(v);
                }
                e.SetValue(values);
            });

            CreatePBData final = new CreatePBData(inHeritNames, "PBData", inFolderName);
            final.SetValue(finalClassNames);
            return true;
        }

        /// <summary>
        /// ------------------- TODO -------------------
        /// </summary>
        /// <returns></returns>
        public bool ExcelToXml(string inPath, string inFolderName)
        {
            Assist.CheckFolderExist(inFolderName);
            Assist.GetObjPaths(".xls", inPath).ForEach(delegate (string path)
            {
                DataSet ds = Assist.ExcelToData(path);
                DataTable dt = ds.Tables[0];

                //ExcelToXml e = new ExcelToXml();

            });
            return true;
        }

        /// <summary>
        /// Excel -> 二进制文件
        /// </summary>
        /// <param name="inPath"></param>
        /// <param name="inFolderName"></param>
        /// <returns></returns>
        public bool Bin(string inPath, string inFolderName)
        {
            Assist.CheckFolderExist(inFolderName);

            PBData data = new PBData();

            Assist.GetObjPaths(".xls", inPath).ForEach(delegate(string path)
            {
                DataSet ds = Assist.ExcelToData(path);
                DataTable dt = ds.Tables[0];

                string classname = Stringer.FirstLetterUp(dt.TableName);

                Type classType = Type.GetType(classname);

                FieldInfo fieldInfo = typeof(PBData).GetField(classname + "Dic");

                IDictionary fieldValue = fieldInfo.GetValue(data) as IDictionary;

                if (classType != null)
                {
                    for (int i = 4; i < dt.Rows.Count; i++)
                    {
                        string key = ""; // key

                        List<string> ls = new List<string>();
                        for (int x = 0; x < dt.Columns.Count; x++)
                        {
                            ls.Add(dt.Rows[i][x].ToString());

                            if (dt.Rows[2][x].ToString().Split('+')[2] == "1")// 当前值为Key值的一部分
                            {
                                key += dt.Rows[i][x].ToString() + "_";
                            }
                        }
                        key = key.Remove(key.Length - 1, 1);
                        IProtoBufable value = Activator.CreateInstance(classType) as IProtoBufable;// value
                        value.Set(ls);
                        fieldValue.Add(key, value);
                    }

                    fieldInfo.SetValue(data, fieldValue);
                }
            });

            #region SceneLayout 读取同级目录下的“Excel/SceneLayout/”内的所有 xml 文件，并将其数据写入 PBData

            Dictionary<string, XmlDocument> doc = Assist.GetXml(Assist.RootPath + "Excel/SceneLayout/");
            foreach (KeyValuePair<string, XmlDocument> item in doc)
            {
                SceneLayout sl = new SceneLayout();
                XmlNodeList xcc = item.Value.SelectSingleNode("Config").ChildNodes;
                for (int i = 0; i < xcc.Count; i++)
                {
                    SceneLayoutItem sli = new SceneLayoutItem();

                    IProtoBufable xmlItemValue = new SceneLayoutItem() as IProtoBufable;// value
                    List<string> xls = new List<string>();
                    for (int x = 0; x < xcc[i].Attributes.Count; x++)
                    {
                        xls.Add(xcc[i].Attributes[x].Value);
                    }
                    xmlItemValue.Set(xls);

                    sl.item.Add(xmlItemValue as SceneLayoutItem);
                }
                data.SceneLayoutDic.Add(item.Key, sl);
            }

            #endregion

            using (var file = System.IO.File.Create("PBData"))
            {
                try
                {
                    ProtoBuf.Serializer.Serialize(file, data);
                }
                catch (Exception e)
                {
                    MainWindow.Show(e.ToString());
                }
            }

            return true;
        }

        /// <summary>
        /// 将 .proto 文件转成 .cs 文件
        /// <para>用于项目非ProtoBuf-net类型协议，以后用PB协议的话这里就没有用了</para>
        /// </summary>
        /// <param name="inPath"></param>
        /// <param name="inFolderName"></param>
        /// <returns></returns>
        public bool Proto2CSharp(string inPath, string inFolderName)
        {
            Assist.CheckFolderExist(inFolderName);
            Assist.GetObjPaths(".proto", inPath).ForEach(delegate(string path)
            {
                FileInfo targetFileInfo = new FileInfo(path);
                if (targetFileInfo != null)
                {
                    Queue<string> sq = new Queue<string>();
                    StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
                    String line;
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine().Trim();
                        line = Regex.Replace(line, @"/{2}.+", "");
                        line = Regex.Replace(line, @"{\s*", "");
                        if (line != "")
                            sq.Enqueue(line);
                    }
                    sr.Close();
                    sr.Dispose();

                    string curNameSpace = sq.Dequeue().Remove(0, 8).Replace(";", "");

                    // 类名 -> 成员列表<成员名, 成员类型>
                    Dictionary<string, Dictionary<string, string>> nClasses = HanldQueue(sq);

                    foreach (KeyValuePair<string, Dictionary<string, string>> item in nClasses)
                    {
                        if (item.Key != "")
                        {
                            ProtoToCSharp ptcs = new ProtoToCSharp(curNameSpace, item.Key, "ProtoItem");
                            ptcs.Create(item.Value);
                        }
                    }
                }
            });
            return true;
        }

        private Dictionary<string, Dictionary<string, string>> HanldQueue(Queue<string> inQueue)
        {
            Dictionary<string, Dictionary<string, string>> back = new Dictionary<string, Dictionary<string, string>>();

            string curClassName = "";
            Dictionary<string, string> curMembers = new Dictionary<string, string>();

            while (inQueue.Count > 0)
            {
                string s = inQueue.Dequeue();
                if (s == "")
                {
                    
                }
                else if (s.Contains("message"))
                {
                    curClassName = s.Remove(0, 8).Replace("{", "");
                }
                else if (s.Contains("required"))
                {
                    string[] v = s.Split(' ');
                    curMembers.Add(v[2], v[1]);
                }
                else
                {
                    Dictionary<string, string> ad = new Dictionary<string,string>();
                    foreach (KeyValuePair<string, string> item in curMembers)
                    {
                        ad.Add(item.Key, item.Value);
                    }
                    back.Add(curClassName, ad);
                    curClassName = "";
                    curMembers.Clear();
                }
            }


            return back;
        }
    }
}
