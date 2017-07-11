namespace Krypton.LibProtocol.Type
{
    public class UInt64Type : KryptonType<UInt64Type>
    {
        public ulong Value { get; set; }

        public static implicit operator UInt64Type(ulong val)
        {
            return new UInt64Type { Value = val };
        }
        
        public static implicit operator ulong(UInt64Type val)
        {
            return val.Value;
        }
        
        public override void Write(BufferWriter bw)
        {
            bw.WriteUInt64(Value);
        }

        public override void Consume(BufferReader br)
        {
            Value = br.ReadUInt64();
        }
    }
}
