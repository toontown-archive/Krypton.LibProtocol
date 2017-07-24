namespace Krypton.LibProtocol.Numericals
{
    public struct Int64Type : IKryptonType
    {
        public long Value { get; set; }

        public static implicit operator Int64Type(long val)
        {
            return new Int64Type { Value = val };
        }
        
        public static implicit operator long(Int64Type val)
        {
            return val.Value;
        }
        
        public void Write(BufferWriter bw)
        {
            bw.WriteInt64(Value);
        }

        public void Consume(BufferReader br)
        {
            Value = br.ReadInt64();
        }
        
        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
