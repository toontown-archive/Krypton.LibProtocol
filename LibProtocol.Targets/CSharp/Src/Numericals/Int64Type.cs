using System.IO;

namespace Krypton.LibProtocol.Numericals
{
    public struct Int64Type : IKryptonType
    {
        public static implicit operator Int64Type(long val)
        {
            return new Int64Type { Value = val };
        }
        
        public static implicit operator long(Int64Type val)
        {
            return val.Value;
        }

        public long Value;
        
        public void Write(BinaryWriter bw)
        {
            bw.Write(Value);
        }

        public void Read(BinaryReader br)
        {
            Value = br.ReadInt64();
        }
    }
}
