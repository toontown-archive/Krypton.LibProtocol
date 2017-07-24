namespace Krypton.LibProtocol.Numericals
{
    public struct Int16Type : IKryptonType
    {
        public short Value { get; set; }

        public static implicit operator Int16Type(short val)
        {
            return new Int16Type { Value = val };
        }
        
        public static implicit operator short(Int16Type val)
        {
            return val.Value;
        }
        
        public void Write(BufferWriter bw)
        {
            bw.WriteInt16(Value);
        }

        public void Consume(BufferReader br)
        {
            Value = br.ReadInt16();
        }
        
        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
