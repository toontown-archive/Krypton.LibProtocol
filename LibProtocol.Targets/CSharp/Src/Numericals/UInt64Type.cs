using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;

namespace Krypton.LibProtocol.Numericals
{
    public struct UInt64Type : IKryptonType
    {
        public static implicit operator UInt64Type(ulong val)
        {
            return new UInt64Type { Value = val };
        }
        
        public static implicit operator ulong(UInt64Type val)
        {
            return val.Value;
        }
        
        public ulong Value;

        public void Write(BinaryWriter bw)
        {
            bw.Write(Value);
        }

        public void Read(BinaryReader br)
        {
            Value = br.ReadUInt64();
        }
    }
}
