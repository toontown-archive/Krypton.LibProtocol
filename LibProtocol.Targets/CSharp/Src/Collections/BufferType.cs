using System.IO;

namespace Krypton.LibProtocol.Collections
{
    public struct BufferType : IKryptonType
    {
        public static implicit operator BufferType(byte[] val)
        {
            return new BufferType { Value = val };
        }
        
        public static implicit operator byte[](BufferType val)
        {
            return val.Value;
        }
        
        public byte[] Value;
        
        public void Write(BinaryWriter bw)
        {
            bw.Write(Value, 0 , Value.Length);
        }

        public void Read(BinaryReader br)
        {
            Value = new byte[br.BaseStream.Length];
            br.Read(Value, 0, Value.Length);
        }
    }
}
