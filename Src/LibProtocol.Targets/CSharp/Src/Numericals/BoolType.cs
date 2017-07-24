namespace Krypton.LibProtocol.Numericals
{
    public struct BoolType : IKryptonType
    {
        public bool Value { get; set; }
        
        public static implicit operator BoolType(bool val)
        {
            return new BoolType { Value = val };
        }

        public static implicit operator bool(BoolType val)
        {
            return val.Value;
        }

        public void Write(BufferWriter bw)
        {
            bw.WriteBool(Value);
        }

        public void Consume(BufferReader br)
        {
            Value = br.ReadBool();
        }
        
        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
