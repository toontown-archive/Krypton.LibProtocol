namespace Krypton.LibProtocol.Type
{
    public class StringType : KryptonType<StringType>
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
        
        public override void Write(BufferWriter bw)
        {
            bw.WriteString(Value);
        }

        public override void Consume(BufferReader br)
        {
            Value = br.ReadString();
        }
        
        public override void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
