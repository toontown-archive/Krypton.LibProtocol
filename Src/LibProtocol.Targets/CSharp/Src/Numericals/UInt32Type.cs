using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Krypton.LibProtocol.Numericals
{
    public struct UInt32Type : IKryptonType
    {
        public static implicit operator UInt32Type(uint val)
        {
            return new UInt32Type { Value = val };
        }
        
        public static implicit operator uint(UInt32Type val)
        {
            return val.Value;
        }
        
        public uint Value;

        public void Write(BinaryWriter bw)
        {
            bw.Write(Value);
        }

        public void Read(BinaryReader br)
        {
            Value = br.ReadUInt32();
        }
    }
}
