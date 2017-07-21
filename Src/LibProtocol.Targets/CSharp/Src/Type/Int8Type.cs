namespace Krypton.LibProtocol.Type
{
    public class Int8Type : KryptonType<Int8Type>
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
        
        public override void Write(BufferWriter bw)
        {
            bw.WriteInt8(Value);
        }

        public override void Consume(BufferReader br)
        {
            Value = br.ReadInt8();
        }
        
        public override void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
