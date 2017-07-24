namespace Krypton.LibProtocol.Collections
{
    public struct StringType : IKryptonType
    {
        public string Value { get; set; }

        public static implicit operator StringType(string val)
        {
            return new StringType { Value = val };
        }
        
        public static implicit operator string(StringType val)
        {
            return val.Value;
        }
        
        public void Write(BufferWriter bw)
        {
            bw.WriteString(Value);
        }

        public void Consume(BufferReader br)
        {
            Value = br.ReadString();
        }
        
        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
