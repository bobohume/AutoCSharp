/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：SceneLayout
 * 简    述： 
 * 创建时间：2015/8/18 15:05:08
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using ProtoBuf;
using System.Collections.Generic;

[ProtoContract()]
public class SceneLayout
{
    private List<SceneLayoutItem> _items = new List<SceneLayoutItem>();
        
    [ProtoMember(1)]
    public List<SceneLayoutItem> item
    {
        get { return _items; }
        set { _items = value; }
    }
}
