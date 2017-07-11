namespace Krypton.LibProtocol.Type
{
    public class ByteType : KryptonType<ByteType>
    {
        public byte Value { get; set; }

        public static implicit operator ByteType(byte val)
        {
            return new ByteType { Value = val };
        }
        
        public static implicit operator byte(ByteType val)
        {
            return val.Value;
        }
        
        public override void Write(BufferWriter bw)
        {
            bw.WriteByte(Value);
        }

        public override void Consume(BufferReader br)
        {
            Value = br.ReadByte();
        }
    }
}
