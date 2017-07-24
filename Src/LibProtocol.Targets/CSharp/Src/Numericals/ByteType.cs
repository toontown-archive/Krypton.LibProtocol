namespace Krypton.LibProtocol.Numericals
{
    public struct ByteType : IKryptonType
    {
        public byte Value { get; set; }

        public static implicit operator ByteType(byte val)
        {
            return new ByteType { Value = val };
        }
        
        public static implicit operator byte(ByteType val)
        {
            return val.Value;
        }
        
        public void Write(BufferWriter bw)
        {
            bw.WriteByte(Value);
        }

        public void Consume(BufferReader br)
        {
            Value = br.ReadByte();
        }
        
        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
