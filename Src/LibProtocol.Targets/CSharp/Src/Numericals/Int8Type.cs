namespace Krypton.LibProtocol.Numericals
{
    public struct Int8Type : IKryptonType
    {
        public sbyte Value { get; set; }

        public static implicit operator Int8Type(sbyte val)
        {
            return new Int8Type { Value = val };
        }
        
        public static implicit operator sbyte(Int8Type val)
        {
            return val.Value;
        }
        
        public void Write(BufferWriter bw)
        {
            bw.WriteInt8(Value);
        }

        public void Consume(BufferReader br)
        {
            Value = br.ReadInt8();
        }
        
        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
