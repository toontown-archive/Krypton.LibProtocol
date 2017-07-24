namespace Krypton.LibProtocol.Numericals
{
    public struct UInt64Type : IKryptonType
    {
        public ulong Value { get; set; }

        public static implicit operator UInt64Type(ulong val)
        {
            return new UInt64Type { Value = val };
        }
        
        public static implicit operator ulong(UInt64Type val)
        {
            return val.Value;
        }
        
        public void Write(BufferWriter bw)
        {
            bw.WriteUInt64(Value);
        }

        public void Consume(BufferReader br)
        {
            Value = br.ReadUInt64();
        }
        
        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
