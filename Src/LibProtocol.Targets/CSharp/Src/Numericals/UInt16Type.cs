using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace Krypton.LibProtocol.Numericals
{
    public struct UInt16Type : IKryptonType
    {
        public static implicit operator UInt16Type(ushort val)
        {
            return new UInt16Type { Value = val };
        }
        
        public static implicit operator ushort(UInt16Type val)
        {
            return val.Value;
        }
        
        public ushort Value;
        
        public void Write(BinaryWriter bw)
        {
            bw.Write(Value);
        }

        public void Read(BinaryReader br)
        {
            Value = br.ReadUInt16();
        }
    }
}
