namespace Krypton.LibProtocol.Type
{
    public class UInt8Type : KryptonType<UInt8Type>
    {
        public byte Value { get; set; }

        public static implicit operator UInt8Type(byte val)
        {
            return new UInt8Type { Value = val };
        }
        
        public static implicit operator byte(UInt8Type val)
        {
            return val.Value;
        }
        
        public override void Write(BufferWriter bw)
        {
            bw.WriteUInt8(Value);
        }

        public override void Consume(BufferReader br)
        {
            Value = br.ReadUInt8();
        }
    }
}
