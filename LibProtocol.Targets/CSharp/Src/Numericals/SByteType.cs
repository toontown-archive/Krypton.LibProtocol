using System.IO;

namespace Krypton.LibProtocol.Numericals
{
    public struct SByteType : IKryptonType
    {
        public static implicit operator SByteType(sbyte val)
        {
            return new SByteType { Value = val };
        }
        
        public static implicit operator sbyte(SByteType val)
        {
            return val.Value;
        }

        public sbyte Value;
        
        public void Write(BinaryWriter bw)
        {
            bw.Write(Value);
        }

        public void Read(BinaryReader br)
        {
            Value = br.ReadSByte();
        }
    }
}
