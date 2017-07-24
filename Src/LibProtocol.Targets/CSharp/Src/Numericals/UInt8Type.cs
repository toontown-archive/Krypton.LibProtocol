namespace Krypton.LibProtocol.Numericals
{
    public struct UInt8Type : IKryptonType
    {
        public byte Value { get; set; }

        public static implicit operator UInt8Type(byte val)
        {
            return new UInt8Type { Value = val };
        }
        
        public static implicit operator byte(UInt8Type val)
        {
            return val.Value;
        }
        
        public void Write(BufferWriter bw)
        {
            bw.WriteUInt8(Value);
        }

        public void Consume(BufferReader br)
        {
            Value = br.ReadUInt8();
        }
        
        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
