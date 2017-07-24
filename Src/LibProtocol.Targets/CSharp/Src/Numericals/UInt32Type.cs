namespace Krypton.LibProtocol.Numericals
{
    public struct UInt32Type : IKryptonType
    {
        public uint Value { get; set; }

        public static implicit operator UInt32Type(uint val)
        {
            return new UInt32Type { Value = val };
        }
        
        public static implicit operator uint(UInt32Type val)
        {
            return val.Value;
        }
        
        public void Write(BufferWriter bw)
        {
            bw.WriteUInt32(Value);
        }

        public void Consume(BufferReader br)
        {
            Value = br.ReadUInt32();
        }
        
        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
