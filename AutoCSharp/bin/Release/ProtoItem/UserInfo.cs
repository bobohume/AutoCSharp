//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace KLData
{
    using System;
    
    
    public class UserInfo : Stream
    {
        
        public int age;
        
        public int bookCount;
        
        public int[] books;
        
        public override byte[] ToBytes()
        {
            ByteArray bytes = new ByteArray();
            bytes.Write(this.age);
            bytes.Write(this.bookCount);
            for (int i = 0; (i < books.bookCount); i = (i + 1))
            {
                bytes.Write(this.books[i]);
            }
            return bytes.ToBytes();
        }
        
        public static Stream ToStream(byte[] inArg0)
        {
            UserInfo stream = new UserInfo();
            ByteArray data = new ByteArray(inArg0);
            stream.age = data.ReadInt();
            stream.bookCount = data.ReadInt();
            stream.books = new System.Int32[bookCount];
            for (int i = 0; (i < stream.bookCount); i = (i + 1))
            {
                stream.books[i] = data.ReadInt();
            }
            return stream;
        }
    }
}
