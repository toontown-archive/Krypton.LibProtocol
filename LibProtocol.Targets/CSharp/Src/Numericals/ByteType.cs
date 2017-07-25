using System.IO;

namespace Krypton.LibProtocol.Numericals
{
    public struct ByteType : IKryptonType
    {
        public static implicit operator ByteType(byte val)
        {
            return new ByteType { Value = val };
        }
        
        public static implicit operator byte(ByteType val)
        {
            return val.Value;
        }

        public byte Value;

        public void Write(BinaryWriter bw)
        {
            bw.Write(Value);
        }

        public void Read(BinaryReader br)
        {
            Value = br.ReadByte();
        }
    }
}
