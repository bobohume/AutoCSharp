/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：SceneLayout
 * 简    述： 
 * 创建时间：2015/8/18 14:47:22
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using ProtoBuf;
using System.Collections.Generic;

[ProtoContract()]
public class SceneLayoutItem : IProtoBufable
{
    private int _id;

    private string _name;

    private string _path;

    private Vector3Unit _position;

    private Vector3Unit _rotation;

    private Vector3Unit _scale;

    private int _lightmapIndex;

    private Vector4Unit _lightmapScaleOffset;

    private string _mainTexturePath;
    /// <summary>
    /// id
    /// </summary>
    [ProtoMember(1)]
    public int id
    {
        get { return this._id; }
        set { this._id = value; }
    }
    /// <summary>
    /// 预置物的名字
    /// </summary>
    [ProtoMember(2)]
    public string name
    {
        get { return this._name; }
        set { this._name = value; }
    }
    /// <summary>
    /// AB包所在路径
    /// </summary>
    [ProtoMember(3)]
    public string path
    {
        get { return this._path; }
        set { this._path = value; }
    }
    /// <summary>
    /// 位置
    /// </summary>
    [ProtoMember(4)]
    public Vector3Unit position
    {
        get { return this._position; }
        set { this._position = value; }
    }
    /// <summary>
    /// 旋转
    /// </summary>
    [ProtoMember(5)]
    public Vector3Unit rotation
    {
        get { return this._rotation; }
        set { this._rotation = value; }
    }
    /// <summary>
    /// 缩放
    /// </summary>
    [ProtoMember(6)]
    public Vector3Unit scale
    {
        get { return this._scale; }
        set { this._scale = value; }
    }
    /// <summary>
    /// 光照信息序号
    /// </summary>
    [ProtoMember(7)]
    public int lightmapIndex
    {
        get { return _lightmapIndex; }
        set { _lightmapIndex = value; }
    }
    /// <summary>
    /// 光照信息偏移值
    /// </summary>
    [ProtoMember(8)]
    public Vector4Unit lightmapScaleOffset
    {
        get { return _lightmapScaleOffset; }
        set { _lightmapScaleOffset = value; }
    }
    /// <summary>
    /// 纹理贴图路径
    /// </summary>
    [ProtoMember(9)]
    public string mainTexturePath
    {
        get { return _mainTexturePath; }
        set { _mainTexturePath = value; }
    }

    public string key
    {
        get
        {
            return this._id.ToString();
        }
    }

    public void Set(List<string> inArg0)
    {
        _id = int.Parse(inArg0[0]);
        _name = inArg0[1];
        _path = inArg0[2];
        _position = new Vector3Unit(inArg0[3]);
        _rotation = new Vector3Unit(inArg0[4]);
        _scale = new Vector3Unit(inArg0[5]);
        _lightmapIndex = int.Parse(inArg0[6]);
        _lightmapScaleOffset = new Vector4Unit(inArg0[7]);
        _mainTexturePath = inArg0[8];
    }
}
