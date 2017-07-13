namespace Krypton.LibProtocol.Type
{
    public class Int64Type : KryptonType<Int64Type>
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
        
        public override void Write(BufferWriter bw)
        {
            bw.WriteInt64(Value);
        }

        public override void Consume(BufferReader br)
        {
            Value = br.ReadInt64();
        }
    }
}
