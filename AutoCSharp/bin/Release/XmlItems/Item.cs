//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace KLBase.Xml
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    
    
    public class Item : IXmlValue
    {
        
        private uint _itemID;
        
        private uint _itemQuality;
        
        private string _name;
        
        private string _note;
        
        private uint _type;
        
        private uint _giftOpen;
        
        private uint _lastTime;
        
        private uint _maxCount;
        
        private uint _iconName;
        
        private string _itemDesc;
        
        private uint _sellPrice;
        
        public uint itemID
        {
            get
            {
                return this._itemID;
            }
        }
        
        public uint itemQuality
        {
            get
            {
                return this._itemQuality;
            }
        }
        
        public string name
        {
            get
            {
                return this._name;
            }
        }
        
        public string note
        {
            get
            {
                return this._note;
            }
        }
        
        public uint type
        {
            get
            {
                return this._type;
            }
        }
        
        public uint giftOpen
        {
            get
            {
                return this._giftOpen;
            }
        }
        
        public uint lastTime
        {
            get
            {
                return this._lastTime;
            }
        }
        
        public uint maxCount
        {
            get
            {
                return this._maxCount;
            }
        }
        
        public uint iconName
        {
            get
            {
                return this._iconName;
            }
        }
        
        public string itemDesc
        {
            get
            {
                return this._itemDesc;
            }
        }
        
        public uint sellPrice
        {
            get
            {
                return this._sellPrice;
            }
        }
        
        public void SetValue(Dictionary<string, string> inArg0)
        {
            if (inArg0.ContainsKey("itemID"))
            {
                if (inArg0["itemID"] == "")
                {
                    this._itemID = 0;
                }
                else
                {
                    this._itemID = uint.Parse(inArg0["itemID"]);
                }
            }
            if (inArg0.ContainsKey("itemQuality"))
            {
                if (inArg0["itemQuality"] == "")
                {
                    this._itemQuality = 0;
                }
                else
                {
                    this._itemQuality = uint.Parse(inArg0["itemQuality"]);
                }
            }
            if (inArg0.ContainsKey("name"))
            {
                this._name = inArg0["name"];
            }
            if (inArg0.ContainsKey("note"))
            {
                this._note = inArg0["note"];
            }
            if (inArg0.ContainsKey("type"))
            {
                if (inArg0["type"] == "")
                {
                    this._type = 0;
                }
                else
                {
                    this._type = uint.Parse(inArg0["type"]);
                }
            }
            if (inArg0.ContainsKey("giftOpen"))
            {
                if (inArg0["giftOpen"] == "")
                {
                    this._giftOpen = 0;
                }
                else
                {
                    this._giftOpen = uint.Parse(inArg0["giftOpen"]);
                }
            }
            if (inArg0.ContainsKey("lastTime"))
            {
                if (inArg0["lastTime"] == "")
                {
                    this._lastTime = 0;
                }
                else
                {
                    this._lastTime = uint.Parse(inArg0["lastTime"]);
                }
            }
            if (inArg0.ContainsKey("maxCount"))
            {
                if (inArg0["maxCount"] == "")
                {
                    this._maxCount = 0;
                }
                else
                {
                    this._maxCount = uint.Parse(inArg0["maxCount"]);
                }
            }
            if (inArg0.ContainsKey("iconName"))
            {
                if (inArg0["iconName"] == "")
                {
                    this._iconName = 0;
                }
                else
                {
                    this._iconName = uint.Parse(inArg0["iconName"]);
                }
            }
            if (inArg0.ContainsKey("itemDesc"))
            {
                this._itemDesc = inArg0["itemDesc"];
            }
            if (inArg0.ContainsKey("sellPrice"))
            {
                if (inArg0["sellPrice"] == "")
                {
                    this._sellPrice = 0;
                }
                else
                {
                    this._sellPrice = uint.Parse(inArg0["sellPrice"]);
                }
            }
        }
    }
}
