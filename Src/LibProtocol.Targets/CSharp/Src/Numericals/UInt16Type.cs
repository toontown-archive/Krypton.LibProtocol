namespace Krypton.LibProtocol.Numericals
{
    public struct UInt16Type : IKryptonType
    {
        public ushort Value { get; set; }

        public static implicit operator UInt16Type(ushort val)
        {
            return new UInt16Type { Value = val };
        }
        
        public static implicit operator ushort(UInt16Type val)
        {
            return val.Value;
        }
        
        public void Write(BufferWriter bw)
        {
            bw.WriteUInt16(Value);
        }

        public void Consume(BufferReader br)
        {
            Value = br.ReadUInt16();
        }
        
        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
