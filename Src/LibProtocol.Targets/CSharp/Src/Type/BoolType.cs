namespace Krypton.LibProtocol.Type
{
    public class BoolType : KryptonType<BoolType>
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
        
        public override void Write(BufferWriter bw)
        {
            bw.WriteBool(Value);
        }

        public override void Consume(BufferReader br)
        {
            Value = br.ReadBool();
        }
        
        public override void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
